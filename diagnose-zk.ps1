# Standalone ZKTeco protocol diagnostic
# Tests the TCP handshake directly without the Bridge

param(
    [string]$Ip = "192.168.1.201",
    [int]$Port = 4370,
    [int]$TimeoutMs = 5000
)

Write-Host "=== ZKTeco Protocol Diagnostic ===" -ForegroundColor Cyan
Write-Host "Target: $Ip`:$Port"
Write-Host ""

$USHRT_MAX = 65535

function ToHex {
    param([byte[]]$Bytes)
    if ($null -eq $Bytes -or $Bytes.Length -eq 0) { return "(empty)" }
    $sb = New-Object System.Text.StringBuilder
    foreach ($b in $Bytes) { [void]$sb.Append($b.ToString("X2")) }
    return $sb.ToString()
}

function Build-Packet {
    param([int]$Command, [byte[]]$Data, [int]$SessionId, [int]$ReplyId)
    
    # payload = [command(2) + checksum(2) + session(2) + replyId(2) + data]
    $payload = New-Object byte[] (8 + $Data.Length)
    [System.BitConverter]::GetBytes([uint16]$Command).CopyTo($payload, 0)
    # checksum placeholder at [2..3]
    [System.BitConverter]::GetBytes([uint16]$SessionId).CopyTo($payload, 4)
    [System.BitConverter]::GetBytes([uint16]$ReplyId).CopyTo($payload, 6)
    $Data.CopyTo($payload, 8)
    
    # Calculate checksum
    [uint32]$chk = 0
    for ($i = 0; $i + 1 -lt $payload.Length; $i += 2) {
        $chk = $chk + [System.BitConverter]::ToUInt16($payload, $i)
    }
    if ($payload.Length % 2 -ne 0) {
        $chk = $chk + $payload[$payload.Length - 1]
    }
    $chk = ($chk -shr 16) + ($chk -band 0xFFFF)
    $chk = $chk + ($chk -shr 16)
    $chkval = [uint16]((-bnot $chk) -band 0xFFFF)
    [System.BitConverter]::GetBytes($chkval).CopyTo($payload, 2)
    
    # TCP top header: [Magic1(2) + Magic2(2) + payloadLen(4)]
    $Magic1 = [uint16]0x5050
    $Magic2 = [uint16]0x7D82
    $packet = New-Object byte[] (8 + $payload.Length)
    [System.BitConverter]::GetBytes($Magic1).CopyTo($packet, 0)
    [System.BitConverter]::GetBytes($Magic2).CopyTo($packet, 2)
    [System.BitConverter]::GetBytes([uint32]$payload.Length).CopyTo($packet, 4)
    $payload.CopyTo($packet, 8)
    
    return $packet
}

function Make-CommKey {
    param([int]$Key, [int]$SessionId)
    [uint32]$k = 0
    for ($i = 0; $i -lt 32; $i++) {
        if (($Key -band (1 -shl $i)) -ne 0) { $k = ($k -shl 1) -bor 1 }
        else { $k = $k -shl 1 }
    }
    $k += [uint32]$SessionId
        $kb = [System.BitConverter]::GetBytes($k)
        $kb[0] = $kb[0] -bxor [byte][char]'Z'
        $kb[1] = $kb[1] -bxor [byte][char]'K'
        $kb[2] = $kb[2] -bxor [byte][char]'S'
        $kb[3] = $kb[3] -bxor [byte][char]'O'
        $tmp = $kb[0]; $kb[0] = $kb[2]; $kb[2] = $tmp
        $tmp = $kb[1]; $kb[1] = $kb[3]; $kb[3] = $tmp
        $b = [byte]50
        $kb[0] = $kb[0] -bxor $b
        $kb[1] = $kb[1] -bxor $b
        $kb[3] = $kb[3] -bxor $b
        return @($kb[0], $kb[1], $b, $kb[3])
}

function Send-Recv {
    param([System.Net.Sockets.TcpClient]$Tcp, [byte[]]$Packet, [int]$ReadTimeoutMs = 3000)
    
    try {
        $stream = $Tcp.GetStream()
        $stream.Write($Packet, 0, $Packet.Length)
        Write-Host "  Sent: $(ToHex($Packet))" -ForegroundColor Gray
        
        # Read 8-byte header
        $header = New-Object byte[] 8
        $offset = 0
        $sw = [System.Diagnostics.Stopwatch]::StartNew()
        while ($offset -lt 8) {
            if ($sw.ElapsedMilliseconds -gt $ReadTimeoutMs) { 
                Write-Host "  ERROR: Timeout reading header" -ForegroundColor Red
                return $null 
            }
            $read = $stream.Read($header, $offset, 8 - $offset)
            if ($read -eq 0) {
                Write-Host "  ERROR: Connection closed" -ForegroundColor Red
                return $null
            }
            $offset += $read
        }
        $sw.Stop()
        
        $magic1 = [System.BitConverter]::ToUInt16($header, 0)
        $magic2 = [System.BitConverter]::ToUInt16($header, 2)
        $payloadSize = [System.BitConverter]::ToUInt32($header, 4)
        
        Write-Host "  Header: magic1=0x$('{0:X4}' -f $magic1) magic2=0x$('{0:X4}' -f $magic2) payloadSize=$payloadSize" -ForegroundColor Yellow
        
        if ($payloadSize -eq 0) { return @{} }
        if ($payloadSize -gt 65535) {
            Write-Host "  ERROR: Invalid payload size" -ForegroundColor Red
            return $null
        }
        
        # Read payload
        $payload = New-Object byte[] $payloadSize
        $offset = 0
        while ($offset -lt $payloadSize) {
            $read = $stream.Read($payload, $offset, $payloadSize - $offset)
            if ($read -eq 0) { 
                Write-Host "  ERROR: Connection closed reading payload" -ForegroundColor Red
                return $null 
            }
            $offset += $read
        }
        
        Write-Host "  Payload: $(ToHex($payload))" -ForegroundColor Gray
        
        $respCode = [System.BitConverter]::ToUInt16($payload, 0)
        $respSid = [System.BitConverter]::ToUInt16($payload, 4)
        $respRid = [System.BitConverter]::ToUInt16($payload, 6)
        $respData = $payload[8..($payload.Length - 1)]
        
        return @{
            Code = $respCode
            SessionId = $respSid
            ReplyId = $respRid
            Data = $respData
            DataHex = ToHex($respData)
        }
    }
    catch {
        Write-Host "  ERROR: $_" -ForegroundColor Red
        return $null
    }
}

# Main diagnostic
try {
    $tcp = New-Object System.Net.Sockets.TcpClient
    Write-Host "[1] Connecting TCP..." -ForegroundColor Green
    $async = $tcp.BeginConnect($Ip, $Port, $null, $null)
    if (-not $async.AsyncWaitHandle.WaitOne($TimeoutMs)) {
        Write-Host "FAILED: TCP connection timeout" -ForegroundColor Red
        exit 1
    }
    $tcp.EndConnect($async)
    $tcp.ReceiveTimeout = 5000
    $tcp.SendTimeout = 5000
    Write-Host "OK: TCP connected" -ForegroundColor Green
    
    $replyId = $USHRT_MAX - 1
    $sessionId = 0
    
    # Step 1: CMD_CONNECT
    Write-Host ""
    Write-Host "[2] CMD_CONNECT (1000)..." -ForegroundColor Green
    $replyId = ($replyId + 1) -band 0xFFFF
    $packet = Build-Packet -Command 1000 -Data @() -SessionId 0 -ReplyId $replyId
    $resp = Send-Recv -Tcp $tcp -Packet $packet
    if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
    
    Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
    Write-Host "  Data hex: $($resp.DataHex)" -ForegroundColor Cyan
    
    $sessionId = $resp.SessionId
    $replyId = $resp.ReplyId
    
    # Step 2: CMD_AUTH (if needed)
    if ($resp.Code -eq 2005) {
        Write-Host ""
        Write-Host "[3] CMD_AUTH (1102) - device requires auth..." -ForegroundColor Green
        $commKey = Make-CommKey -Key 0 -SessionId $sessionId
        Write-Host "  CommKey hex: $(ToHex($commKey))" -ForegroundColor Gray
        $replyId = ($replyId + 1) -band 0xFFFF
        $packet = Build-Packet -Command 1102 -Data $commKey -SessionId $sessionId -ReplyId $replyId
        $resp = Send-Recv -Tcp $tcp -Packet $packet
        if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
        Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
        Write-Host "  Data hex: $($resp.DataHex)" -ForegroundColor Cyan
        $sessionId = $resp.SessionId
        $replyId = $resp.ReplyId
    }
    
    if ($resp.Code -ne 2000) {
        Write-Host "FAILED: Expected CMD_ACK_OK (2000), got $($resp.Code)" -ForegroundColor Red
        exit 1
    }
    Write-Host "OK: Connected successfully (session=$sessionId)" -ForegroundColor Green
    
    # Step 3: CMD_GET_FREE_SIZES
    Write-Host ""
    Write-Host "[4] CMD_GET_FREE_SIZES (50)..." -ForegroundColor Green
    $replyId = ($replyId + 1) -band 0xFFFF
    $packet = Build-Packet -Command 50 -Data @() -SessionId $sessionId -ReplyId $replyId
    $resp = Send-Recv -Tcp $tcp -Packet $packet
    if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
    Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
    Write-Host "  Data len: $($resp.Data.Length) hex: $($resp.DataHex)" -ForegroundColor Cyan
    
    # Step 4: CMD_GET_VERSION
    Write-Host ""
    Write-Host "[5] CMD_GET_VERSION (1100)..." -ForegroundColor Green
    $replyId = ($replyId + 1) -band 0xFFFF
    $packet = Build-Packet -Command 1100 -Data @() -SessionId $sessionId -ReplyId $replyId
    $resp = Send-Recv -Tcp $tcp -Packet $packet
    if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
    Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
    if ($resp.Data.Length -gt 0) { Write-Host "  Data text: $([System.Text.Encoding]::ASCII.GetString($resp.Data))" -ForegroundColor Cyan }
    Write-Host "  Data hex: $($resp.DataHex)" -ForegroundColor Cyan
    
    # Step 5: CMD_REG_EVENT (500)
    Write-Host ""
    Write-Host "[6] CMD_REG_EVENT (500) with flags=0xFFFF..." -ForegroundColor Green
    $evtData = New-Object byte[] 4
    [System.BitConverter]::GetBytes([int32]0xFFFF).CopyTo($evtData, 0)
    $replyId = ($replyId + 1) -band 0xFFFF
    $packet = Build-Packet -Command 500 -Data $evtData -SessionId $sessionId -ReplyId $replyId
    $resp = Send-Recv -Tcp $tcp -Packet $packet
    if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
    Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
    Write-Host "  Data hex: $($resp.DataHex)" -ForegroundColor Cyan
    
    # Step 6: CMD_PREPARE_BUFFER (1503) with inner_cmd=CMD_USERTEMP_RRQ(9), fct=FCT_USER(5)
    Write-Host ""
    Write-Host "[7] CMD_PREPARE_BUFFER (1503) inner=9 fct=5..." -ForegroundColor Green
    # PREPARE_BUFFER: 11 bytes: [1(version) + cmd(2) + fct(4) + ext(4)]
    $bufData = New-Object byte[] 11
    $bufData[0] = 1  # version
    [System.BitConverter]::GetBytes([uint16]9).CopyTo($bufData, 1)   # CMD_USERTEMP_RRQ
    [System.BitConverter]::GetBytes([int32]5).CopyTo($bufData, 3)    # FCT_USER
    [System.BitConverter]::GetBytes([int32]0).CopyTo($bufData, 7)    # ext
    $replyId = ($replyId + 1) -band 0xFFFF
    $packet = Build-Packet -Command 1503 -Data $bufData -SessionId $sessionId -ReplyId $replyId
    $resp = Send-Recv -Tcp $tcp -Packet $packet
    if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
    Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
    if ($resp.Data.Length -gt 0) { Write-Host "  Data len: $($resp.Data.Length) hex: $($resp.DataHex)" -ForegroundColor Cyan }
    
    # Step 7: CMD_PREPARE_BUFFER (1503) with inner_cmd=CMD_ATTLOG_RRQ(13), fct=FCT_ATTLOG(1)
    Write-Host ""
    Write-Host "[8] CMD_PREPARE_BUFFER (1503) inner=13 fct=1..." -ForegroundColor Green
    $bufData = New-Object byte[] 11
    $bufData[0] = 1  # version
    [System.BitConverter]::GetBytes([uint16]13).CopyTo($bufData, 1)  # CMD_ATTLOG_RRQ
    [System.BitConverter]::GetBytes([int32]1).CopyTo($bufData, 3)    # FCT_ATTLOG
    [System.BitConverter]::GetBytes([int32]0).CopyTo($bufData, 7)    # ext
    $replyId = ($replyId + 1) -band 0xFFFF
    $packet = Build-Packet -Command 1503 -Data $bufData -SessionId $sessionId -ReplyId $replyId
    $resp = Send-Recv -Tcp $tcp -Packet $packet
    if ($null -eq $resp) { Write-Host "FAILED" -ForegroundColor Red; exit 1 }
    Write-Host "  Response: code=$($resp.Code) sid=$($resp.SessionId) rid=$($resp.ReplyId)" -ForegroundColor Cyan
    if ($resp.Data.Length -gt 0) { Write-Host "  Data len: $($resp.Data.Length) hex: $($resp.DataHex)" -ForegroundColor Cyan }
    
    $tcp.Close()
    Write-Host ""
    Write-Host "=== Diagnostic Complete ===" -ForegroundColor Green
}
catch {
    Write-Host "FATAL: $_" -ForegroundColor Red
}

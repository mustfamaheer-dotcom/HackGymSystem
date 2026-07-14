<# 
  ZKTeco Integration Test Script (v2)
  Tests the full pipeline: Bridge -> API -> Database -> Frontend
  
  Prerequisites:
    1. Bridge running: dotnet run --project src/HackGym.ZKTeco.Bridge/ -c Release
    2. API running:    dotnet run --project src/Gym.API/ -c Release --urls http://localhost:5000
    3. ZKTeco device on network at 192.168.1.201:4370 (or configured IP)
#>

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   ZKTeco MB2000 Integration Test v2" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$API = "http://localhost:5000"
$BRIDGE = "http://localhost:50051"
$API_KEY = "zkteco-bridge-secret-2026"

# Config
$ADMIN_USER = if ($env:ADMIN_USERNAME) { $env:ADMIN_USERNAME } else { "admin" }
$ADMIN_PASS = if ($env:ADMIN_PASSWORD) { $env:ADMIN_PASSWORD } else { "Admin@123" }

$headers = @{ "X-API-Key" = $API_KEY }

function Write-Step($num, $label) {
    Write-Host "`n=== $num. $label ===" -ForegroundColor Yellow
}

function Write-Result($ok, $msg) {
    $color = if ($ok) { "Green" } else { "Red" }
    $prefix = if ($ok) { "[OK]" } else { "[FAIL]" }
    Write-Host "   $prefix $msg" -ForegroundColor $color
}

# 1. BRIDGE HEALTH
Write-Step 1 "Bridge Health Check"
try {
    $r = Invoke-WebRequest -Uri "$BRIDGE/zkteco.bridge.ZKTecoBridge/CheckHealth" -Method POST `
        -ContentType "application/json" -Body "{}" -UseBasicParsing -TimeoutSec 10
    $data = $r.Content | ConvertFrom-Json
    Write-Result $data.isConnected "Bridge connected to device: $($data.isConnected)"
    Write-Host "   Users on device: $($data.enrolledUserCount)" -ForegroundColor Gray
    Write-Host "   Firmware: $($data.firmwareVersion)" -ForegroundColor Gray
} catch {
    Write-Result $false "Bridge unreachable: $_`n   Start the Bridge: dotnet run --project src/HackGym.ZKTeco.Bridge/ -c Release"
}

# 2. BRIDGE TEST CONNECTION
Write-Step 2 "Bridge Connection Test"
try {
    $r = Invoke-WebRequest -Uri "$BRIDGE/zkteco.bridge.ZKTecoBridge/TestConnection" -Method POST `
        -ContentType "application/json" -Body "{}" -UseBasicParsing -TimeoutSec 20
    $data = $r.Content | ConvertFrom-Json
    Write-Result $data.success "Round-trip: $($data.roundTripLatencyMs)ms"
    if (-not $data.success) {
        Write-Host "   Error: $($data.errorMessage)" -ForegroundColor Yellow
    }
} catch {
    Write-Result $false "Test connection failed: $_"
}

# 3. API HEALTH
Write-Step 3 "API ZKTeco Health"
try {
    $r = Invoke-WebRequest -Uri "$API/api/zkteco-attendance/health" -Method GET `
        -Headers $headers -UseBasicParsing -TimeoutSec 10
    $data = $r.Content | ConvertFrom-Json
    Write-Result $true "Service: $($data.service), Bridge connected: $($data.bridgeConnected)"
} catch {
    Write-Result $false "Health check failed: $_"
}

# 4. BRIDGE DIAGNOSE USERS
Write-Step 4 "Bridge User Diagnostics"
try {
    $r = Invoke-WebRequest -Uri "$BRIDGE/zkteco.bridge.ZKTecoBridge/DiagnoseUsers" -Method POST `
        -ContentType "application/json" -Body "{}" -UseBasicParsing -TimeoutSec 30
    $data = $r.Content | ConvertFrom-Json
    if ($data.connected) {
        Write-Result $true "Users on device: $($data.userCount)"
        if ($data.sampleUsers) {
            $data.sampleUsers | ForEach-Object {
                Write-Host "   - ID: $($_.enrollmentId), Name: $($_.name), Priv: $($_.privilege)" -ForegroundColor Gray
            }
        }
    } else {
        Write-Result $false "Device not connected: $($data.error)"
    }
} catch {
    Write-Result $false "Diagnose failed: $_"
}

# 5. BRIDGE RAW PROTOCOL DIAGNOSTICS
Write-Step 5 "Bridge Raw Protocol Diagnostics"
try {
    $r = Invoke-WebRequest -Uri "$BRIDGE/diagnose/raw" -Method GET -UseBasicParsing -TimeoutSec 30
    $data = $r.Content | ConvertFrom-Json
    if ($data.connected) {
        Write-Result $true "Protocol diagnostics collected"
        $data.diagnostics | Get-Member -MemberType Properties | ForEach-Object {
            $key = $_.Name
            $val = $data.diagnostics.$key
            $status = if ($val.code -eq 2000 -or $val.code -eq 2002 -or $val.dataLen -gt 0) { "OK" } else { "CHECK" }
            Write-Host "   [$status] $key => code=$($val.code), dataLen=$($val.dataLen)" -ForegroundColor Gray
        }
    } else {
        Write-Result $false "Device not connected"
    }
} catch {
    Write-Result $false "Raw diagnostic failed: $_"
}

# 6. API SYNC USERS
Write-Step 6 "API Sync Users from Device"
try {
    $r = Invoke-WebRequest -Uri "$API/api/zkteco-attendance/sync-users" -Method POST `
        -ContentType "application/json" -Headers $headers `
        -Body "[]" -UseBasicParsing -TimeoutSec 30
    $data = $r.Content | ConvertFrom-Json
    Write-Result $data.success "Sync result: processed=$($data.totalProcessed), created=$($data.createdCount), synced=$($data.syncedCount), skipped=$($data.skippedCount)"
} catch {
    Write-Result $false "Sync failed: $_"
}

# 7. API IMPORT ALL FROM DEVICE
Write-Step 7 "API Import All From Device"
try {
    $r = Invoke-WebRequest -Uri "$API/api/zkteco-attendance/import-all-from-device" -Method POST `
        -Headers $headers -UseBasicParsing -TimeoutSec 30
    $data = $r.Content | ConvertFrom-Json
    if ($data.success -or $data.message) {
        Write-Result $true "$($data.message)"
    } else {
        Write-Result $false "$($data.error)"
    }
} catch {
    Write-Result $false "Import failed: $_"
}

# 8. TRACK MEMBERS STATS (requires auth)
Write-Step 8 "Track Members Stats (authenticated)"
try {
    $loginBody = @{ Username = $ADMIN_USER; Password = $ADMIN_PASS } | ConvertTo-Json
    $loginResp = Invoke-WebRequest -Uri "$API/api/auth/login" -Method POST -Body $loginBody `
        -ContentType "application/json" -SessionVariable S -UseBasicParsing -TimeoutSec 10
    $loginData = $loginResp.Content | ConvertFrom-Json
    
    $r = Invoke-WebRequest -Uri "$API/api/track-members/stats" -Method GET `
        -WebSession $S -UseBasicParsing -TimeoutSec 10
    $data = $r.Content | ConvertFrom-Json
    Write-Result $data.success "Total Members: $($data.data.totalMembers)"
    Write-Host "   Checked in today: $($data.data.checkedInToday)" -ForegroundColor Gray
    Write-Host "   Absent: $($data.data.absentToday)" -ForegroundColor Gray
    Write-Host "   Late: $($data.data.lateToday)" -ForegroundColor Gray
    Write-Host "   Devices online: $($data.data.devicesOnline)" -ForegroundColor Gray
} catch {
    Write-Result $false "Stats failed: $_"
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   Test Complete" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor White
Write-Host "  1. Scan a finger on the MB2000 device" -ForegroundColor Gray
Write-Host "  2. The Bridge polls every 3s and sends to /api/zkteco-attendance/push" -ForegroundColor Gray
Write-Host "  3. Check API logs: logs/attendance-yyyyMMdd-.log" -ForegroundColor Gray
Write-Host "  4. Visit Track Members page to see live feed" -ForegroundColor Gray
Write-Host "  5. To simulate: POST /api/track-members/simulate with { enrollmentId, direction(0=in/1=out) }" -ForegroundColor Gray

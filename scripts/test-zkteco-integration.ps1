<# 
  ZKTeco Integration Test Script
  Run this after restarting the API: 
    1. Close the old API terminal window
    2. Start fresh: dotnet run --project src/Gym.API/ -c Release --urls http://localhost:5000
    3. Run this script
#>

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   ZKTeco MB2000 Integration Test" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$BASE = "http://localhost:5000"

# 1. LOGIN
Write-Host "=== 1. Login ===" -ForegroundColor Yellow
$loginBody = @{ Username = "admin"; Password = "Admin@123" } | ConvertTo-Json
try {
    $loginResp = Invoke-WebRequest -Uri "$BASE/api/Auth/login" -Method POST -Body $loginBody `
        -ContentType "application/json" -SessionVariable S -UseBasicParsing -TimeoutSec 10
    Write-Host "   [OK] Login successful" -ForegroundColor Green
} catch {
    Write-Host "   [FAIL] Login failed: $_" -ForegroundColor Red
    exit 1
}

# 2. TEST DEVICE CONNECTION
Write-Host "`n=== 2. Test Device Connection ===" -ForegroundColor Yellow
try {
    $r = Invoke-WebRequest -Uri "$BASE/api/ZKTeco/testconnection" -Method POST `
        -WebSession $S -UseBasicParsing -TimeoutSec 20
    $data = $r.Content | ConvertFrom-Json
    if ($data.succeeded) {
        Write-Host "   [OK] Device reachable: $($data.data.connected)" -ForegroundColor Green
        Write-Host "   Round-trip: $($data.data.roundTripMs)ms" -ForegroundColor Gray
        Write-Host "   Firmware: $($data.data.firmwareVersion)" -ForegroundColor Gray
    } else {
        Write-Host "   [WARN] $($data.message)" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   [FAIL] Connection test failed: $_" -ForegroundColor Red
}

# 3. DEVICE STATUS
Write-Host "`n=== 3. Device Status ===" -ForegroundColor Yellow
try {
    $r = Invoke-WebRequest -Uri "$BASE/api/ZKTeco/status" -Method GET `
        -WebSession $S -UseBasicParsing -TimeoutSec 20
    $data = $r.Content | ConvertFrom-Json
    if ($data.succeeded) {
        Write-Host "   [OK] Device: $($data.data.deviceName)" -ForegroundColor Green
        Write-Host "   IP: $($data.data.ip):$($data.data.port)" -ForegroundColor Gray
        Write-Host "   Uptime: $($data.data.uptime)" -ForegroundColor Gray
        Write-Host "   Users on device: $($data.data.userCount)" -ForegroundColor Gray
        Write-Host "   Free space: $($data.data.freeSpace)" -ForegroundColor Gray
    }
} catch {
    Write-Host "   [FAIL] Status check failed: $_" -ForegroundColor Red
}

# 4. RECONCILE USERS (sync subscriptions to device)
Write-Host "`n=== 4. Reconcile Users ===" -ForegroundColor Yellow
try {
    $r = Invoke-WebRequest -Uri "$BASE/api/ZKTeco/reconcile" -Method POST `
        -WebSession $S -UseBasicParsing -TimeoutSec 60
    $data = $r.Content | ConvertFrom-Json
    if ($data.succeeded) {
        Write-Host "   [OK] Reconciliation complete" -ForegroundColor Green
        Write-Host "   Synced: $($data.data.synced)" -ForegroundColor Gray
        Write-Host "   Failed: $($data.data.failed)" -ForegroundColor Gray
        Write-Host "   Skipped: $($data.data.skipped)" -ForegroundColor Gray
    }
} catch {
    Write-Host "   [FAIL] Reconciliation failed: $_" -ForegroundColor Red
}

# 5. SYNC LOGS
Write-Host "`n=== 5. Sync Audit Logs ===" -ForegroundColor Yellow
try {
    $r = Invoke-WebRequest -Uri "$BASE/api/ZKTeco/sync-logs?page=1&pageSize=5" -Method GET `
        -WebSession $S -UseBasicParsing -TimeoutSec 10
    $data = $r.Content | ConvertFrom-Json
    if ($data.succeeded) {
        Write-Host "   [OK] Total sync events: $($data.data.total)" -ForegroundColor Green
        $data.data.items | ForEach-Object {
            Write-Host "   - $($_.eventType): $($_.status) at $($_.timestamp)" -ForegroundColor Gray
        }
    }
} catch {
    Write-Host "   [FAIL] Sync logs failed: $_" -ForegroundColor Red
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   Test Complete" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next: Scan a finger on the MB2000 device -" -ForegroundColor White
Write-Host "the Bridge polls every 3s and sends to /api/attendances/check-in" -ForegroundColor Gray

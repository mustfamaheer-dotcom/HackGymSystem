# Start both Gym API and ZKTeco Bridge in parallel
# Run this from the repo root: .\start-all.ps1

$ErrorActionPreference = "Continue"

# Config — override via env vars or defaults
$JWT_SECRET = if ($env:JWT__Secret) { $env:JWT__Secret } else { "DevJwtSecretKeyThatIsAtLeast32CharsLong!!" }
$ADMIN_PASS = if ($env:Seed__AdminPassword) { $env:Seed__AdminPassword } else { "Admin@123" }
$CONN_STR  = if ($env:ConnectionStrings__DefaultConnection) { $env:ConnectionStrings__DefaultConnection } else { "Data Source=GymDb.db; Cache=Shared;" }

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Starting Hack Gym System" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# --- Build both projects first ---
Write-Host "[1/4] Building Gym API..." -ForegroundColor Yellow
dotnet build src/Gym.API/Gym.API.csproj -c Release --nologo 2>&1 | Out-Null

Write-Host "[2/4] Building ZKTeco Bridge..." -ForegroundColor Yellow
dotnet build src/HackGym.ZKTeco.Bridge/HackGym.ZKTeco.Bridge.csproj -c Release --nologo 2>&1 | Out-Null

# --- Start ZKTeco Bridge ---
Write-Host "[3/4] Starting ZKTeco Bridge (port 50051)..." -ForegroundColor Yellow
$bridgeJob = Start-Job -Name "ZKTecoBridge" -ScriptBlock {
    $env:JWT__Secret = $using:JWT_SECRET
    $env:Seed__AdminPassword = $using:ADMIN_PASS
    $env:ConnectionStrings__DefaultConnection = $using:CONN_STR
    dotnet run --project src/HackGym.ZKTeco.Bridge/HackGym.ZKTeco.Bridge.csproj -c Release --no-build
}
Start-Sleep 3

# --- Start Gym API ---
Write-Host "[4/4] Starting Gym API (port 5000)..." -ForegroundColor Yellow
$apiJob = Start-Job -Name "GymAPI" -ScriptBlock {
    $env:JWT__Secret = $using:JWT_SECRET
    $env:Seed__AdminPassword = $using:ADMIN_PASS
    $env:ConnectionStrings__DefaultConnection = $using:CONN_STR
    dotnet run --project src/Gym.API/Gym.API.csproj -c Release --no-build
}

Start-Sleep 5

# --- Verify ---
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Checking services..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

try {
    $r = Invoke-WebRequest -Uri "http://localhost:50051/zkteco.bridge.ZKTecoBridge/CheckHealth" -Method POST -ContentType "application/json" -Body "{}" -UseBasicParsing -TimeoutSec 5
    Write-Host "  [OK] ZKTeco Bridge: $($r.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "  [WARN] ZKTeco Bridge: not reachable (still starting?)" -ForegroundColor Yellow
}

try {
    $r = Invoke-WebRequest -Uri "http://localhost:5000/health" -UseBasicParsing -TimeoutSec 5
    Write-Host "  [OK] Gym API: $($r.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "  [WARN] Gym API: not yet reachable" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Both services started in background." -ForegroundColor Cyan
Write-Host "  Open: http://localhost:5000" -ForegroundColor White
Write-Host "  Login: admin / $ADMIN_PASS" -ForegroundColor Gray
Write-Host ""
Write-Host "  To stop:" -ForegroundColor Gray
Write-Host "    Stop-Job -Name ZKTecoBridge" -ForegroundColor Gray
Write-Host "    Stop-Job -Name GymAPI" -ForegroundColor Gray
Write-Host "========================================" -ForegroundColor Cyan

# Keep jobs alive — script stays resident
Write-Host "`nPress Ctrl+C to stop all services." -ForegroundColor DarkYellow
while ($true) {
    Start-Sleep 10
    # Check if jobs are still running
    $bridgeRunning = (Get-Job -Name "ZKTecoBridge" -ErrorAction SilentlyContinue).State -eq "Running"
    $apiRunning = (Get-Job -Name "GymAPI" -ErrorAction SilentlyContinue).State -eq "Running"
    if (-not $bridgeRunning -or -not $apiRunning) {
        Write-Host "[WARN] One or more services stopped unexpectedy." -ForegroundColor Yellow
        if (-not $bridgeRunning) { Write-Host "  - ZKTeco Bridge stopped" -ForegroundColor Red }
        if (-not $apiRunning) { Write-Host "  - Gym API stopped" -ForegroundColor Red }
        break
    }
}

<#
.SYNOPSIS
    Builds a production-ready single-file deployment for the Gym Management System.

.DESCRIPTION
    - Publishes Gym.API as a self-contained, single-file .exe (win-x64).
    - Copies appsettings.json, wwwroot, and an empty zkemkeeper/ folder next to the .exe.
    - Drops run.bat, install-service.bat, uninstall-service.bat, README.txt next to it.
    - Scrubs *.pdb files from the publish output.

    Run from the repository root:  D:\Hack gym system
#>

[CmdletBinding()]
param(
    [switch]$SkipPublish,
    [switch]$SkipZkemkeeper
)

# ---------------------------------------------------------------------------
# 0. Resolve paths and switch to the repository root
# ---------------------------------------------------------------------------
$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot    = Resolve-Path (Join-Path $scriptRoot '') | Select-Object -ExpandProperty Path
Set-Location $repoRoot

$sln          = Join-Path $repoRoot 'GymManagement.slnx'
$apiProject   = Join-Path $repoRoot 'src\Gym.API\Gym.API.csproj'
$srcWww       = Join-Path $repoRoot 'src\Gym.API\wwwroot'
$srcSettings  = Join-Path $repoRoot 'src\Gym.API\appsettings.json'
$deployRoot   = Join-Path $repoRoot 'deploy'
$deployDir    = Join-Path $deployRoot 'GymSystem'

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Gym Management System - Build & Deploy" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Repo root : $repoRoot"
Write-Host "API proj  : $apiProject"
Write-Host "Output    : $deployDir"
Write-Host ""

# ---------------------------------------------------------------------------
# 1. Edge case: .NET 10 SDK must be installed on the build machine
# ---------------------------------------------------------------------------
$dotnet = (Get-Command dotnet -ErrorAction SilentlyContinue)
if (-not $dotnet) {
    Write-Host "[ERROR] .NET SDK is not installed (or not in PATH)." -ForegroundColor Red
    Write-Host "        Install .NET SDK 10 from https://dotnet.microsoft.com/download/dotnet/10.0" -ForegroundColor Red
    Write-Host "        Then re-run this script." -ForegroundColor Red
    exit 1
}
$dotnetVersion = & dotnet --version 2>$null
Write-Host "[OK] .NET SDK detected: $dotnetVersion"

if ($dotnetVersion -notlike '10.*') {
    Write-Host "[WARN] Expected .NET 10.x SDK. Found: $dotnetVersion" -ForegroundColor Yellow
    Write-Host "        Proceeding anyway - if publish fails, install .NET 10 SDK." -ForegroundColor Yellow
}

# ---------------------------------------------------------------------------
# 2. Restore + clean + publish
# ---------------------------------------------------------------------------
if (Test-Path $deployRoot) {
    Write-Host "[CLEAN] Removing old $deployRoot" -ForegroundColor Yellow
    Remove-Item $deployRoot -Recurse -Force
}

if (-not $SkipPublish) {
    Write-Host ""
    Write-Host "[PUBLISH] dotnet publish (single-file, self-contained, win-x64)..." -ForegroundColor Cyan
    & dotnet publish $apiProject `
        --configuration Release `
        --runtime win-x64 `
        --self-contained true `
        --output $deployDir `
        /p:PublishSingleFile=true `
        /p:IncludeNativeLibrariesForSelfExtract=true `
        /p:PublishTrimmed=false `
        /p:DebugType=none `
        /p:DebugSymbols=false `
        /p:PublishReadyToRun=true `
        /p:EnableCompressionInSingleFile=true

    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] dotnet publish failed with exit code $LASTEXITCODE" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host "[OK] Publish completed: $deployDir"
} else {
    if (-not (Test-Path $deployDir)) {
        Write-Host "[ERROR] -SkipPublish was passed but $deployDir does not exist." -ForegroundColor Red
        exit 1
    }
    Write-Host "[OK] -SkipPublish: reusing existing $deployDir"
}

# ---------------------------------------------------------------------------
# 3. Scrub debug symbols (*.pdb) and dev-only artifacts
# ---------------------------------------------------------------------------
Get-ChildItem -Path $deployDir -Recurse -Filter '*.pdb' -ErrorAction SilentlyContinue | ForEach-Object {
    Remove-Item $_.FullName -Force -ErrorAction SilentlyContinue
    Write-Host "[CLEAN] Removed $($_.FullName)"
}
# Remove dev-only config files that leak into publish
foreach ($devFile in @('appsettings.Development.json')) {
    $p = Join-Path $deployDir $devFile
    if (Test-Path $p) { Remove-Item $p -Force; Write-Host "[CLEAN] Removed $devFile" }
}

# ---------------------------------------------------------------------------
# 4. Replace appsettings.json with our production-safe defaults
#     (Jwt.Secret stays in env vars in production; we keep appsettings minimal.)
# ---------------------------------------------------------------------------
$productionSettings = @'
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://0.0.0.0:5000"
      }
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=GymDb.db; Cache=Shared;"
  },
  "Jwt": {
    "Issuer": "GymManagementAPI",
    "Audience": "GymManagementApp",
    "AccessTokenExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    },
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/gym-api-.log",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
          "retainedFileCountLimit": 14
        }
      }
    ]
  },
  "AllowedHosts": "*"
}
'@

$deploySettings = Join-Path $deployDir 'appsettings.json'
if (Test-Path $deploySettings) { Remove-Item $deploySettings -Force }
$productionSettings | Set-Content -Path $deploySettings -Encoding UTF8
Write-Host "[OK] Wrote production appsettings.json"

# ---------------------------------------------------------------------------
# 5. Ensure wwwroot exists
# ---------------------------------------------------------------------------
$deployWww = Join-Path $deployDir 'wwwroot'
if (-not (Test-Path $deployWww)) {
    New-Item -ItemType Directory -Path $deployWww | Out-Null
    $placeholder = Join-Path $deployWww 'index.html'
    Set-Content -Path $placeholder -Encoding UTF8 -Value '<!doctype html><html><head><meta charset="utf-8"><title>Hack Gym</title></head><body><h1>System is starting...</h1><p>Please return to <a href="/Account/Login">/Account/Login</a>.</p></body></html>'
    Write-Host "[OK] wwwroot created with placeholder index.html"
} else {
    Write-Host "[OK] wwwroot present"
}

# ---------------------------------------------------------------------------
# 6. Create empty zkemkeeper/ folder for the user to drop the COM DLL into
# ---------------------------------------------------------------------------
if (-not $SkipZkemkeeper) {
    $zkDir = Join-Path $deployDir 'zkemkeeper'
    New-Item -ItemType Directory -Path $zkDir -Force | Out-Null
    $zkReadme = Join-Path $zkDir 'PLACE_ZKEMKEEPER_DLL_HERE.txt'
    if (-not (Test-Path $zkReadme)) {
        Set-Content -Path $zkReadme -Encoding UTF8 -Value @'
Place the file "zkemkeeper.dll" from your ZKTeco SDK in this folder.

After copying it, register the COM component by running (as Administrator):
    regsvr32 "%~dp0zkemkeeper\zkemkeeper.dll"

This is only needed if you use the optional Gym.DeviceService to talk to the
ZKTeco MB2000 fingerprint device. The GymSystem itself does not need it;
you can leave this folder empty if you do not use fingerprint attendance.
'@
    }
    Write-Host "[OK] zkemkeeper/ folder ready (drop zkemkeeper.dll here before using the DeviceService)"
}

# ---------------------------------------------------------------------------
# 7. Copy run.bat, install-service.bat, uninstall-service.bat, README.txt
# ---------------------------------------------------------------------------
$deploySupport = Join-Path $repoRoot 'deploy-support'
if (Test-Path $deploySupport) {
    foreach ($f in @('run.bat', 'install-service.bat', 'uninstall-service.bat', 'README.txt')) {
        $src = Join-Path $deploySupport $f
        $dst = Join-Path $deployDir $f
        Copy-Item $src $dst -Force
        Write-Host "[OK] Copied $f"
    }
} else {
    Write-Host "[WARN] deploy-support/ folder not found at $deploySupport - skipping bat/readme copy" -ForegroundColor Yellow
}

# ---------------------------------------------------------------------------
# 8. Validation / sanity checks
# ---------------------------------------------------------------------------
Write-Host ""
Write-Host "[VALIDATE] Checking the deploy package..."
$exePath = Join-Path $deployDir 'Gym.API.exe'
if (-not (Test-Path $exePath)) {
    Write-Host "[FAIL] $exePath not found." -ForegroundColor Red
    exit 1
}
$exeSize = (Get-Item $exePath).Length
$exeSizeMB = [math]::Round($exeSize / 1MB, 1)
Write-Host ("  - Gym.API.exe : {0} MB" -f $exeSizeMB)
if ($exeSizeMB -lt 20) {
    Write-Host "    [WARN] Single-file exe is unusually small; was PublishSingleFile actually applied?" -ForegroundColor Yellow
}

foreach ($f in @('appsettings.json', 'run.bat', 'install-service.bat', 'uninstall-service.bat', 'README.txt')) {
    $p = Join-Path $deployDir $f
    if (Test-Path $p) { Write-Host "  - $f : OK" }
    else { Write-Host "  - $f : MISSING" -ForegroundColor Red }
}

if (Test-Path $deployWww) {
    $wwwChildren = (Get-ChildItem $deployWww -ErrorAction SilentlyContinue).Count
    Write-Host "  - wwwroot/ : OK ($wwwChildren items)"
}

$totalSize = (Get-ChildItem $deployDir -Recurse | Measure-Object -Property Length -Sum).Sum
$totalMB = [math]::Round($totalSize / 1MB, 1)
Write-Host ("  - Total package size : {0} MB" -f $totalMB)

Write-Host ""
Write-Host "[SENSITIVE-VALUES CHECK] Scanning appsettings.json for dev-only secrets..." -ForegroundColor Cyan
$settingsBody = Get-Content $deploySettings -Raw
$hits = @()
if ($settingsBody -match 'SuperSecret' -or $settingsBody -match 'debug.*true') { $hits += 'dev-secret/debug-true present' }
if ($settingsBody -match 'ASPNETCORE_ENVIRONMENT.*Development') { $hits += 'ASPNETCORE_ENVIRONMENT=Development' }
if ($hits.Count -gt 0) {
    Write-Host "  [WARN] Found potentially dev values: $($hits -join '; ')" -ForegroundColor Yellow
    Write-Host "         For production, edit appsettings.json and rotate the JWT secret (env: JWT__Secret)." -ForegroundColor Yellow
} else {
    Write-Host "  [OK] No obvious dev-only secrets found in appsettings.json" -ForegroundColor Green
}

Write-Host ""
Write-Host "[DONE] Deploy package ready at: $deployDir" -ForegroundColor Green
Write-Host "       - Copy that folder to the gym owner's PC."
Write-Host "       - They double-click run.bat (no install needed)."
Write-Host "       - Right-click appsettings.json -> Edit to change DB / port / etc."

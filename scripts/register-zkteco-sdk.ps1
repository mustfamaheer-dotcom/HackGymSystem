# Run this script as Administrator!
# Right-click PowerShell -> Run as Administrator, then:
#   .\scripts\register-zkteco-sdk.ps1

$sdkDir = "C:\ProgramData\ZKTeco\SDK"
$dllPath = Join-Path $sdkDir "zkemkeeper.dll"

Write-Host "=== ZKTeco MB2000 SDK Registration ===" -ForegroundColor Cyan
Write-Host "SDK Path: $sdkDir" -ForegroundColor Gray

# Check if already registered
$regPath = "HKLM:\SOFTWARE\Classes\CLSID\{00853A19-BD51-419B-9269-2DABE57EB61F}"
$existing = Get-ItemProperty $regPath -ErrorAction SilentlyContinue
if ($existing) {
    Write-Host "ALREADY REGISTERED - CLSID found in registry" -ForegroundColor Yellow
    Write-Host "Re-registering anyway..."
}

# Determine which regsvr32 to use
if ([Environment]::Is64BitOperatingSystem) {
    $regsvr32 = "$env:SystemRoot\SysWOW64\regsvr32.exe"
    Write-Host "64-bit OS detected, using 32-bit regsvr32: $regsvr32" -ForegroundColor Gray
} else {
    $regsvr32 = "$env:SystemRoot\System32\regsvr32.exe"
}

# Register the DLL
Write-Host "Registering $dllPath ..." -ForegroundColor Cyan
$result = & $regsvr32 /s $dllPath 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "SUCCESS: zkemkeeper.dll registered." -ForegroundColor Green
} else {
    Write-Host "FAILED with exit code: $LASTEXITCODE" -ForegroundColor Red
    Write-Host "Possible causes:" -ForegroundColor Yellow
    Write-Host "  1. Not running as Administrator" -ForegroundColor Yellow
    Write-Host "  2. Missing VC++ redistributable (install: vcredist_x86.exe)" -ForegroundColor Yellow
    Write-Host "  3. The DLL is already registered from a different path" -ForegroundColor Yellow
    exit 1
}

# Verify
$check = Get-ItemProperty $regPath -ErrorAction SilentlyContinue
if ($check) {
    Write-Host "VERIFIED: COM component accessible at CLSID {00853A19-BD51-419B-9269-2DABE57EB61F}" -ForegroundColor Green
    Write-Host "All DLLs in SDK directory:" -ForegroundColor Gray
    Get-ChildItem $sdkDir | Select-Object Name, Length | Format-Table -AutoSize
    Write-Host "=== Registration Complete ===" -ForegroundColor Cyan
} else {
    Write-Host "REGISTRATION NOT VERIFIED in registry" -ForegroundColor Red
    exit 1
}

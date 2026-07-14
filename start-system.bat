@echo off
title Hack Gym System Launcher
color 0E
setlocal enabledelayedexpansion

:: =============================================
:: Hack Gym System - Full Setup & Launcher
:: =============================================

cd /d "%~dp0"
set "PROJECT_ROOT=%~dp0"
set "DB_NAME=GymManagementDb"
set "BACKUP_DIR=%PROJECT_ROOT%backups"
set "API_PROJECT=%PROJECT_ROOT%src\Gym.API"
set "BRIDGE_PROJECT=%PROJECT_ROOT%src\HackGym.ZKTeco.Bridge"
set "INFRA_PROJECT=%PROJECT_ROOT%src\Gym.Infrastructure"
set "FRONTEND_DIR=%PROJECT_ROOT%gym-web"
set "WWWROOT=%API_PROJECT%\wwwroot"

if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%" >nul 2>&1

:: -------------------------------------------------
:: Database (SQLite, file-based — no server needed)
:: -------------------------------------------------
set "CONN_STR=Data Source=%PROJECT_ROOT%GymDb.db; Cache=Shared;"
set "DB_INSTANCE=SQLite (GymDb.db)"

:: Set environment variable so all tools and the app use the same connection
set "ConnectionStrings__DefaultConnection=%CONN_STR%"

:: =============================================
:: STEP 0 - Database Menu
:: =============================================
cls
echo.
echo  ===================================================
echo       HACK GYM SYSTEM - Full Setup Launcher
echo  ===================================================
echo.
echo   Database: %DB_INSTANCE%
echo.
:: SQLite needs no external tools for backup/restore (handled via file copy)

:db_menu
echo  ===================================================
echo       DATABASE SETUP
echo  ===================================================
echo.
    echo   [1]  Auto-migrate (EF Core) - Create/update schema
    echo   [2]  Restore from backup   - Recover from a .db file
    echo   [3]  Backup current DB     - Save database to a .db file
echo   [4]  Skip                  - Assume DB already exists
echo.
set "DB_CHOICE="
set /p DB_CHOICE="  Enter choice: "

if "%DB_CHOICE%"=="1" goto db_migrate
if "%DB_CHOICE%"=="4" goto db_done

    if "%DB_CHOICE%"=="2" goto db_restore
    if "%DB_CHOICE%"=="3" goto db_backup

echo.
echo   Invalid choice. Please try again.
echo.
goto db_menu

:: -------------------------------------------------
:db_backup
echo.
echo  [DB] Creating database backup...
set "SRC=%PROJECT_ROOT%GymDb.db"
if not exist "!SRC!" (
    echo   ERROR: GymDb.db not found. Run option [1] Auto-migrate first.
    echo.
    goto db_menu
)
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value 2^>nul') do set "DT=%%I"
if "!DT!"=="" set "DT=00000000_000000"
set "DT=!DT:~0,8!_!DT:~8,6!"
set "BACKUP_FILE=%BACKUP_DIR%\GymDb_!DT!.db"

copy /Y "!SRC!" "!BACKUP_FILE!" >nul
if %ERRORLEVEL% EQU 0 (
    echo   SUCCESS: Backup saved to: !BACKUP_FILE!
) else (
    echo   ERROR: Backup failed.
)
echo.
goto db_menu

:: -------------------------------------------------
:db_restore
echo.
echo  [DB] Available backups in "%BACKUP_DIR%":
echo.
set /a COUNT=0
for %%F in ("%BACKUP_DIR%\*.db") do (
    set /a COUNT+=1
    echo     [!COUNT!] %%~nxF
    set "BFILE_!COUNT!=%%~fF"
)
if !COUNT! EQU 0 (
    echo   No .db backup files found. Place a backup in the backups\ folder.
    echo.
    goto db_menu
)
echo.
echo     [R] Return to menu
echo.
set "RESTORE_CHOICE="
set /p RESTORE_CHOICE="  Enter number to restore (or R to return): "
if /i "!RESTORE_CHOICE!"=="R" goto db_menu

set "RESTORE_FILE=!BFILE_%RESTORE_CHOICE%!"
if "!RESTORE_FILE!"=="" (
    echo   Invalid selection.
    echo.
    goto db_restore
)

echo.
echo   WARNING: This will OVERWRITE the current GymDb.db database!
set "CONFIRM="
set /p CONFIRM="  Type YES to confirm: "
if /i not "!CONFIRM!"=="YES" (
    echo   Restore cancelled.
    echo.
    goto db_menu
)

copy /Y "!RESTORE_FILE!" "%PROJECT_ROOT%GymDb.db" >nul
if %ERRORLEVEL% EQU 0 (
    echo.
    echo   SUCCESS: Database restored from !RESTORE_FILE!
    echo.
    echo   Choose next step:
    echo     [C] Continue to full application startup (build + launch)
    echo     [X] Exit this launcher (database restored, safe to close)
    echo.
    set /p "POST_RESTORE=  Your choice: "
    if /i "!POST_RESTORE!"=="X" (
        echo.
        echo   Database restored. Launcher exiting.
        echo   Run start-system.bat later to start the application.
        pause >nul
        exit /b 0
    )
    goto db_done
) else (
    echo.
    echo   ERROR: Restore failed. Check the backup file and permissions.
    echo.
    echo   Press any key to return to menu...
    pause >nul
    goto db_menu
)

:: -------------------------------------------------
:db_migrate
echo.
echo  [DB] Running EF Core migrations...
dotnet ef database update --project "%INFRA_PROJECT%" --startup-project "%API_PROJECT%"
if %ERRORLEVEL% EQU 0 (
    echo   Database: Migrations applied successfully.
) else (
    echo   WARNING: EF migration reported errors (DB may already be current).
)
echo.
goto db_done

:: -------------------------------------------------
:db_done
echo  Database step complete.
echo.

:: =============================================
:: STEP 1 - Prerequisites
:: =============================================
cls
echo  ===================================================
echo       HACK GYM SYSTEM - Starting Up
echo  ===================================================
echo.
echo [1/7] Verifying prerequisites...

where dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo   ERROR: .NET SDK is not installed.
    echo   Download from: https://dotnet.microsoft.com/download
    goto :fatal
)
for /f "tokens=*" %%V in ('dotnet --version 2^>nul') do set "DOTNET_VER=%%V"
echo   .NET SDK : %DOTNET_VER%

dotnet tool list --global 2>nul | findstr /i "dotnet-ef" >nul
if %ERRORLEVEL% NEQ 0 (
    echo   dotnet-ef: Not found - installing...
    dotnet tool install --global dotnet-ef >nul 2>&1
    if %ERRORLEVEL% EQU 0 (
        echo   dotnet-ef: Installed successfully
    ) else (
        echo   WARNING: dotnet-ef install failed. Migrations will be skipped.
    )
) else (
    echo   dotnet-ef : OK
)

where node >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    for /f "tokens=*" %%V in ('node --version 2^>nul') do echo   Node.js   : %%V
) else (
    echo   Node.js   : Not found - will use pre-built frontend if available
)

:: =============================================
:: STEP 2 - Kill old dotnet processes
:: =============================================
echo.
echo [2/7] Stopping any previous instance...
taskkill /f /fi "WINDOWTITLE eq Gym API*" >nul 2>&1
taskkill /f /im dotnet.exe >nul 2>&1
:: Brief pause to let ports free up
ping -n 3 127.0.0.1 >nul
echo   Done.

:: =============================================
:: STEP 3 - Frontend build (skip if no frontend dir)
:: =============================================
echo.
if not exist "%FRONTEND_DIR%" (
    echo [3/7] Skipping frontend build (gym-web not found)
    echo   The app will serve MVC views from the API directly.
    goto :step4
)
echo [3/7] Building frontend...

where node >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    pushd "%FRONTEND_DIR%"
    if not exist "node_modules" (
        echo   Installing npm dependencies...
        call npm install
        if %ERRORLEVEL% NEQ 0 (
            echo   WARNING: npm install failed.
        )
    )
    if exist "node_modules" (
        echo   Building...
        call npm run build
        if %ERRORLEVEL% EQU 0 (
            echo   Frontend: Built successfully.
        ) else (
            echo   WARNING: npm build failed. Using pre-built dist if available.
        )
    )
    popd
) else (
    echo   Node.js not available - skipping build.
)

if exist "%FRONTEND_DIR%\dist" (
    echo   Frontend dist: Found.
) else (
    echo   WARNING: No dist folder. The app will serve API responses only.
)

:step4

:: =============================================
:: STEP 4 - API build
:: =============================================
echo.
echo [4/7] Building API...

dotnet build "%API_PROJECT%" --nologo -c Release
if %ERRORLEVEL% NEQ 0 (
    echo   Build failed - cleaning and retrying...
    if exist "%API_PROJECT%\obj" rmdir /s /q "%API_PROJECT%\obj" >nul 2>&1
    if exist "%API_PROJECT%\bin" rmdir /s /q "%API_PROJECT%\bin" >nul 2>&1
    dotnet restore "%API_PROJECT%" >nul 2>&1
    dotnet build "%API_PROJECT%" --nologo -c Release
    if %ERRORLEVEL% NEQ 0 (
        echo   ERROR: API build failed. See errors above.
        goto :fatal
    )
)
echo   API: Build complete.

:: =============================================
:: STEP 5 - Bridge build
:: =============================================
echo.
echo [5/7] Building ZKTeco Bridge...

dotnet build "%BRIDGE_PROJECT%" --nologo -c Release
if %ERRORLEVEL% NEQ 0 (
    echo   Bridge build failed - cleaning and retrying...
    if exist "%BRIDGE_PROJECT%\obj" rmdir /s /q "%BRIDGE_PROJECT%\obj" >nul 2>&1
    if exist "%BRIDGE_PROJECT%\bin" rmdir /s /q "%BRIDGE_PROJECT%\bin" >nul 2>&1
    dotnet restore "%BRIDGE_PROJECT%" >nul 2>&1
    dotnet build "%BRIDGE_PROJECT%" --nologo -c Release
    if %ERRORLEVEL% NEQ 0 (
        echo   ERROR: Bridge build failed. Device tracking will be disabled.
        echo   You can still use manual check-in/check-out.
    )
)
echo   Bridge: Build complete.

:: =============================================
:: STEP 6 - Deploy frontend to wwwroot
:: =============================================
echo.
if not exist "%FRONTEND_DIR%" (
    echo [6/7] Skipping frontend deploy (gym-web not found)
    goto :step6
)
echo [6/7] Deploying frontend to wwwroot...
if not exist "%WWWROOT%" mkdir "%WWWROOT%"

if exist "%FRONTEND_DIR%\dist" (
    xcopy /E /Y /I /Q "%FRONTEND_DIR%\dist" "%WWWROOT%" >nul
    echo   Frontend deployed to: %WWWROOT%
) else (
    echo   No dist folder - skipping deploy.
)

:step6

:: =============================================
:: STEP 7 - Generate run scripts and launch
:: =============================================
echo.
echo [7/7] Launching server and bridge...
echo.

:: Write run-api.bat inline (portable, uses relative path)
set "RUN_API=%PROJECT_ROOT%run-api.bat"
(
    echo @echo off
    echo title Gym API Server
    echo color 0A
    echo cd /d "%%~dp0src\Gym.API"
    echo if not defined ConnectionStrings__DefaultConnection ^(
    echo     set "ConnectionStrings__DefaultConnection=Data Source=%PROJECT_ROOT%GymDb.db; Cache=Shared;"
    echo ^)
    echo echo.
    echo echo  ============================================================
    echo echo   Hack Gym - API Server
    echo echo   URL: http://localhost:5000
    echo echo   Login: admin / Admin@123
    echo echo   Press Ctrl+C to stop.
    echo echo  ============================================================
    echo echo.
    echo dotnet run --no-build -c Release --urls "http://0.0.0.0:5000"
    echo echo.
    echo echo  Server stopped.
    echo pause ^>nul
) > "%RUN_API%"

:: Write run-bridge.bat inline (portable, uses relative path)
set "RUN_BRIDGE=%PROJECT_ROOT%run-bridge.bat"
(
    echo @echo off
    echo title ZKTeco Bridge Service
    echo color 0B
    echo cd /d "%%~dp0src\HackGym.ZKTeco.Bridge"
    echo echo.
    echo echo  ============================================================
    echo echo   Hack Gym - ZKTeco Bridge Service
    echo echo   Polling device every 3 seconds...
    echo echo   Press Ctrl+C to stop.
    echo echo  ============================================================
    echo echo.
    echo dotnet run --no-build -c Release
    echo echo.
    echo echo  Bridge stopped.
    echo pause ^>nul
) > "%RUN_BRIDGE%"

echo  ============================================================
echo  Opening browser, starting API and ZKTeco Bridge...
echo  Default login: admin / Admin@123
echo.
echo  Close the respective windows or press Ctrl+C there to stop.
echo  ============================================================
echo.

:: Launch API in its own persistent window
start "Gym API Server" cmd /c ""%RUN_API%""

:: Launch Bridge in its own persistent window
start "ZKTeco Bridge Service" cmd /c ""%RUN_BRIDGE%""

:: Give API a moment to start, then open browser
ping -n 4 127.0.0.1 >nul
start "" http://localhost:5000

echo.
echo  Launcher done. API and Bridge are running in their own windows.
echo  This window can be closed safely.
echo.
pause
goto :eof

:: =============================================
:fatal
echo.
echo  ============================================================
echo   A fatal error occurred. The application cannot start.
echo   Review the errors above, fix them, then re-run this script.
echo  ============================================================
echo.
pause
goto :eof
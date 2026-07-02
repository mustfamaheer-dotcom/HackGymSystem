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
set "SQL_INIT=%PROJECT_ROOT%database\init.sql"
set "API_PROJECT=%PROJECT_ROOT%src\Gym.API"
set "INFRA_PROJECT=%PROJECT_ROOT%src\Gym.Infrastructure"
set "FRONTEND_DIR=%PROJECT_ROOT%gym-web"
set "WWWROOT=%API_PROJECT%\wwwroot"

if not exist "%BACKUP_DIR%" mkdir "%BACKUP_DIR%" >nul 2>&1

:: Check if sqlcmd is available
set "NO_SQLCMD="
where sqlcmd >nul 2>&1
if %ERRORLEVEL% NEQ 0 set "NO_SQLCMD=1"

:: -------------------------------------------------
:: Auto-detect SQL Server (LocalDB or full instance)
:: -------------------------------------------------
set "CONN_STR=Server=localhost;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
set "DB_INSTANCE=localhost"

where sqlcmd >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "SELECT 1" >nul 2>&1
    if %ERRORLEVEL% EQU 0 (
        set "CONN_STR=Server=(localdb)\MSSQLLocalDB;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
        set "DB_INSTANCE=(localdb)\MSSQLLocalDB"
    )
)

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
if defined NO_SQLCMD (
    echo  [NOTE] sqlcmd not found. Backup / Restore / SQL-Init
    echo         options are disabled. Install SSMS to enable them.
    echo.
)

:db_menu
echo  ===================================================
echo       DATABASE SETUP
echo  ===================================================
echo.
echo   [1]  Auto-migrate (EF Core) - Create/update schema
if not defined NO_SQLCMD (
    echo   [2]  Restore from backup   - Recover from a .bak file
    echo   [3]  Backup current DB     - Save database to a .bak file
    echo   [4]  Initialize from SQL   - Run database\init.sql
)
echo   [5]  Skip                  - Assume DB already exists
echo.
set "DB_CHOICE="
set /p DB_CHOICE="  Enter choice: "

if "%DB_CHOICE%"=="1" goto db_migrate
if "%DB_CHOICE%"=="5" goto db_done

if not defined NO_SQLCMD (
    if "%DB_CHOICE%"=="2" goto db_restore
    if "%DB_CHOICE%"=="3" goto db_backup
    if "%DB_CHOICE%"=="4" goto db_init_sql
)

echo.
echo   Invalid choice. Please try again.
echo.
goto db_menu

:: -------------------------------------------------
:db_backup
echo.
echo  [DB] Creating database backup...
for /f "tokens=2 delims==" %%I in ('wmic os get localdatetime /value 2^>nul') do set "DT=%%I"
if "!DT!"=="" set "DT=00000000_000000"
set "DT=!DT:~0,8!_!DT:~8,6!"
set "BACKUP_FILE=%BACKUP_DIR%\%DB_NAME%_!DT!.bak"

sqlcmd -S %DB_INSTANCE% -E -C -Q "BACKUP DATABASE [%DB_NAME%] TO DISK='!BACKUP_FILE:\=\\!' WITH FORMAT, INIT, NAME='GymManagementDb Full Backup'"
if %ERRORLEVEL% EQU 0 (
    echo   SUCCESS: Backup saved to: !BACKUP_FILE!
) else (
    echo   ERROR: Backup failed. Make sure SQL Server is running and the DB exists.
)
echo.
goto db_menu

:: -------------------------------------------------
:db_restore
echo.
echo  [DB] Available backups in "%BACKUP_DIR%":
echo.
set /a COUNT=0
for %%F in ("%BACKUP_DIR%\*.bak") do (
    set /a COUNT+=1
    echo     [!COUNT!] %%~nxF
    set "BFILE_!COUNT!=%%~fF"
)
if !COUNT! EQU 0 (
    echo   No .bak files found. Place a backup in the backups\ folder.
    echo.
    goto db_menu
)
echo.
echo     [R] Return to menu
echo.
set "RESTORE_CHOICE="
set /p RESTORE_CHOICE="  Enter number to restore (or R to return): "
if /i "!RESTORE_CHOICE!"=="R" goto db_menu

:: Validate the number
set "RESTORE_FILE=!BFILE_%RESTORE_CHOICE%!"
if "!RESTORE_FILE!"=="" (
    echo   Invalid selection.
    echo.
    goto db_restore
)

echo.
echo   WARNING: This will OVERWRITE the current [%DB_NAME%] database!
set "CONFIRM="
set /p CONFIRM="  Type YES to confirm: "
if /i not "!CONFIRM!"=="YES" (
    echo   Restore cancelled.
    echo.
    goto db_menu
)

:: Get default data path from SQL Server
set "DATA_PATH="
for /f "usebackq tokens=*" %%P in (`sqlcmd -S %DB_INSTANCE% -E -C -h-1 -W -Q "SET NOCOUNT ON; SELECT SERVERPROPERTY('InstanceDefaultDataPath')" 2^>nul`) do (
    set "DATA_PATH=%%P"
)
if "!DATA_PATH!"=="" set "DATA_PATH=C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\"
if not "!DATA_PATH:~-1!"=="\" set "DATA_PATH=!DATA_PATH!\"

echo.
echo   Restoring: !RESTORE_FILE!
echo   Data path: !DATA_PATH!
echo.

sqlcmd -S %DB_INSTANCE% -E -C -Q "ALTER DATABASE [%DB_NAME%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE" >nul 2>&1
sqlcmd -S %DB_INSTANCE% -E -C -Q "RESTORE DATABASE [%DB_NAME%] FROM DISK='!RESTORE_FILE:\=\\!' WITH MOVE 'GymManagementDb' TO '!DATA_PATH:\=\\!GymManagementDb.mdf', MOVE 'GymManagementDb_log' TO '!DATA_PATH:\=\\!GymManagementDb_log.ldf', REPLACE, RECOVERY"
if %ERRORLEVEL% EQU 0 (
    sqlcmd -S %DB_INSTANCE% -E -C -Q "ALTER DATABASE [%DB_NAME%] SET MULTI_USER" >nul 2>&1
    echo   SUCCESS: Database restored successfully.
    echo.
    echo   Press any key to continue to application startup...
    pause >nul
    goto db_done
) else (
    sqlcmd -S %DB_INSTANCE% -E -C -Q "ALTER DATABASE [%DB_NAME%] SET MULTI_USER" >nul 2>&1
    echo   ERROR: Restore failed. Check backup file and SQL Server logs.
    echo.
    goto db_menu
)

:: -------------------------------------------------
:db_init_sql
echo.
if not exist "%SQL_INIT%" (
    echo   ERROR: File not found: %SQL_INIT%
    echo.
    goto db_menu
)
echo   This will run the full schema + seed script: database\init.sql
set "CONFIRM="
set /p CONFIRM="  Type YES to continue: "
if /i not "!CONFIRM!"=="YES" (
    echo   Skipped.
    echo.
    goto db_menu
)
sqlcmd -S %DB_INSTANCE% -E -C -i "%SQL_INIT%"
if %ERRORLEVEL% EQU 0 (
    echo   SUCCESS: Database initialized.
) else (
    echo   ERROR: SQL script failed. Check SQL Server connectivity.
)
echo.
goto db_menu

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
echo [1/6] Verifying prerequisites...

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
echo [2/6] Stopping any previous instance...
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
    echo [3/6] Skipping frontend build (gym-web not found)
    echo   The app will serve MVC views from the API directly.
    goto :step4
)
echo [3/6] Building frontend...

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
echo [4/6] Building API...

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
:: STEP 5 - Deploy frontend to wwwroot
:: =============================================
echo.
if not exist "%FRONTEND_DIR%" (
    echo [5] Skipping frontend deploy (gym-web not found)
    goto :step6
)
echo [5] Deploying frontend to wwwroot...
if not exist "%WWWROOT%" mkdir "%WWWROOT%"

if exist "%FRONTEND_DIR%\dist" (
    xcopy /E /Y /I /Q "%FRONTEND_DIR%\dist" "%WWWROOT%" >nul
    echo   Frontend deployed to: %WWWROOT%
) else (
    echo   No dist folder - skipping deploy.
)

:step6

:: =============================================
:: STEP 6 - Generate run-api.bat and launch
:: =============================================
echo.
echo [6/6] Launching server...
echo.

:: Write run-api.bat inline (portable, uses relative path)
set "RUN_API=%PROJECT_ROOT%run-api.bat"
(
    echo @echo off
    echo title Gym API Server
    echo color 0A
    echo cd /d "%%~dp0src\Gym.API"
    echo if not defined ConnectionStrings__DefaultConnection ^(
    echo     set "ConnectionStrings__DefaultConnection=Server=(localdb)\MSSQLLocalDB;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
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

echo  ============================================================
echo   Opening browser and starting API in a new window...
echo   Default login: admin / Admin@123
echo.
echo   Close the "Gym API Server" window or press Ctrl+C there to stop.
echo  ============================================================
echo.

:: Launch API in its own persistent window (not /wait so this window stays here)
start "Gym API Server" cmd /c ""%RUN_API%""

:: Give API a moment to start, then open browser
ping -n 4 127.0.0.1 >nul
start "" http://localhost:5000

echo.
echo  Launcher done. The API is running in the "Gym API Server" window.
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
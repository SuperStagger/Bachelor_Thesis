@echo off
:: ==============================================================
::  Запуск "Інформаційна система будівельної компанії"
::  Цей файл викликається з start.vbs — не запускайте напряму.
:: ==============================================================

setlocal EnableDelayedExpansion

set "APP_DIR=%~dp0"
set "APP_EXE=ConstructionCompany.exe"
set "APP_URL=http://localhost:5000"

:: ── Перевірка: чи вже запущено ────────────────────────────────
tasklist /FI "IMAGENAME eq %APP_EXE%" 2>nul | find /I "%APP_EXE%" >nul 2>&1
if %ERRORLEVEL% EQU 0 (
    :: Застосунок вже працює — просто відкрити браузер
    start "" "%APP_URL%"
    exit /b 0
)

:: ── Налаштування середовища ASP.NET Core ──────────────────────
::    Production — вимкнено сторінку детальних помилок
::    ASPNETCORE_URLS — лише HTTP на порту 5000 (без HTTPS)
set ASPNETCORE_ENVIRONMENT=Production
set ASPNETCORE_URLS=%APP_URL%

:: ── Запуск застосунку у фоновому режимі ───────────────────────
cd /d "%APP_DIR%"
start "" /B "%APP_DIR%%APP_EXE%"

:: ── Очікування ініціалізації (EF Core Migrate + Kestrel) ──────
timeout /t 5 /nobreak >nul

:: ── Відкрити браузер ──────────────────────────────────────────
start "" "%APP_URL%"

endlocal

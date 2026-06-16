; ==============================================================
;  Інформаційна система будівельної компанії для покупців
;  Inno Setup 6.x
;  Черкаський національний університет ім. Б. Хмельницького, 2026
;  Студент: Хало Ігор Сергійович  |  Група: КН-22
;  Керівник: Царик Тетяна Юріївна
; ==============================================================
;
;  ПІДГОТОВКА ПЕРЕД КОМПІЛЯЦІЄЮ
;  ─────────────────────────────────────────────────────────────
;  1. Відкрийте термінал у папці, де знаходиться ConstructionCompany.sln
;
;  2. Виконайте команду публікації:
;
;       dotnet publish -c Release --self-contained true ^
;         -r win-x64 -o installer\publish
;
;     (Це створить self-contained збірку: .NET 8 вбудовано,
;      встановлювати Runtime на цільовому ПК не потрібно.)
;
;  3. Переконайтесь, що структура папки виглядає так:
;
;       installer\
;         setup.iss          ← цей файл
;         publish\           ← вивід dotnet publish
;           ConstructionCompany.exe
;           appsettings.json
;           wwwroot\
;           ...
;         launcher\
;           start.bat
;           start.vbs
;           stop.bat
;
;  4. Відкрийте Inno Setup Compiler: File → Open → setup.iss
;  5. Скомпілюйте: Build → Compile (або F9)
;  6. Готовий інсталятор: installer\Output\ConstructionCompany_Setup.exe
;
; ==============================================================

; ── Preprocessor-константи ────────────────────────────────────
#define AppName      "Інформаційна система будівельної компанії"
#define AppVersion   "1.0.0"
#define AppPublisher "Хало І.С. · ЧНУ ім. Б. Хмельницького"
#define AppExeName   "ConstructionCompany.exe"
#define AppPort      "5000"
#define AppURL       "http://localhost:5000"
#define PublishDir   "publish"

; ==============================================================
[Setup]

; ── Ідентифікатор застосунку (не змінювати після першого релізу) ──
AppId={{C7E4F820-3B1A-4D9E-B6C2-91F05A7D3E8B}}

; ── Метадані ──────────────────────────────────────────────────
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} v{#AppVersion}
AppPublisher={#AppPublisher}

; ── Шляхи встановлення ────────────────────────────────────────
DefaultDirName={autopf}\ConstructionCompany
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes

; ── Вихідний файл інсталятора ─────────────────────────────────
OutputDir=Output
OutputBaseFilename=ConstructionCompany_Setup
Compression=lzma2/ultra64
SolidCompression=yes

; ── Зовнішній вигляд майстра ──────────────────────────────────
WizardStyle=modern
WizardSizePercent=120

; ── Права та вимоги ───────────────────────────────────────────
PrivilegesRequired=admin
MinVersion=10.0.17763
CloseApplications=yes

; ── Деінсталяція ──────────────────────────────────────────────
UninstallDisplayIcon={app}\{#AppExeName}
UninstallDisplayName={#AppName}

; ==============================================================
[Languages]
Name: "ukrainian"; MessagesFile: "compiler:Languages\Ukrainian.isl"

; ==============================================================
[Messages]
ukrainian.WelcomeLabel1=Вас вітає Майстер встановлення{nl}{#AppName}
ukrainian.WelcomeLabel2=Буде встановлено версію {#AppVersion} програмного продукту.{nl}{nl}Рекомендується закрити всі інші програми перед продовженням.
ukrainian.FinishedLabel=Встановлення {#AppName} успішно завершено!{nl}{nl}Для запуску використовуйте ярлик на Робочому столі або у меню «Пуск».{nl}{nl}Система відкриється у браузері за адресою:{nl}{#AppURL}

; ==============================================================
[CustomMessages]
ukrainian.TaskDesktopIcon=Створити ярлик на &Робочому столі
ukrainian.LocalDBMissing=SQL Server LocalDB не знайдено!{nl}{nl}Для роботи системи необхідно попередньо встановити{nl}SQL Server Express LocalDB.{nl}{nl}Завантажте за посиланням:{nl}https://go.microsoft.com/fwlink/?linkid=866658{nl}{nl}Після встановлення LocalDB запустіть цей інсталятор повторно.

; ==============================================================
[Tasks]
Name: "desktopicon"; Description: "{cm:TaskDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

; ==============================================================
[Files]

; ── Self-contained застосунок (.NET 8 вбудовано) ──────────────
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; ── Скрипти запуску / зупинки ─────────────────────────────────
Source: "launcher\start.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "launcher\start.vbs"; DestDir: "{app}"; Flags: ignoreversion
Source: "launcher\stop.bat";  DestDir: "{app}"; Flags: ignoreversion

; ==============================================================
[Dirs]
; Папка завантажених зображень — НЕ видаляти при деінсталяції
Name: "{app}\wwwroot\uploads"; Flags: uninsneveruninstall

; ==============================================================
[Icons]

; ── Меню «Пуск» ───────────────────────────────────────────────
Name: "{group}\{#AppName}";           Filename: "{app}\start.vbs"; WorkingDir: "{app}"; IconFilename: "{app}\{#AppExeName}"
Name: "{group}\Зупинити систему";     Filename: "{app}\stop.bat";  WorkingDir: "{app}"
Name: "{group}\Видалити {#AppName}";  Filename: "{uninstallexe}"

; ── Робочий стіл ──────────────────────────────────────────────
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\start.vbs"; WorkingDir: "{app}"; IconFilename: "{app}\{#AppExeName}"; Tasks: desktopicon

; ==============================================================
[Run]
; Запустити систему одразу після встановлення (галочка у майстрі)
Filename: "{app}\start.vbs"; Description: "Запустити {#AppName} зараз"; Flags: postinstall nowait shellexec

; ==============================================================
[UninstallRun]
; Зупинити процес перед видаленням файлів
Filename: "{sys}\taskkill.exe"; Parameters: "/F /IM {#AppExeName}"; Flags: runhidden waituntilterminated; RunOnceId: "StopApp"

; ==============================================================
[Code]

const
  LOCALDB_KEY     = 'SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions';
  LOCALDB_KEY_WOW = 'SOFTWARE\WOW6432Node\Microsoft\Microsoft SQL Server Local DB\Installed Versions';

{ ── Перевірка наявності SQL Server LocalDB ─────────────────── }
function IsLocalDBInstalled: Boolean;
var
  subkeys: TArrayOfString;
begin
  Result := False;

  { Перевірка у 64-bit гілці реєстру }
  if RegGetSubkeyNames(HKLM, LOCALDB_KEY, subkeys) then
    if GetArrayLength(subkeys) > 0 then
    begin
      Result := True;
      Exit;
    end;

  { Запасна перевірка у WOW6432Node (для 32-bit LocalDB на 64-bit ОС) }
  if RegGetSubkeyNames(HKLM, LOCALDB_KEY_WOW, subkeys) then
    if GetArrayLength(subkeys) > 0 then
      Result := True;
end;

{ ── Перевірки на початку встановлення ──────────────────────── }
function InitializeSetup: Boolean;
begin
  Result := True;

  if not IsLocalDBInstalled then
  begin
    MsgBox(CustomMessage('LocalDBMissing'), mbError, MB_OK);
    Result := False;
  end;
end;

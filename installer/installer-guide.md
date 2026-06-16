# Збірка інсталятора — Покрокова інструкція

## Передумови

| Що потрібно | Де взяти |
|---|---|
| **Inno Setup 6.x** | https://jrsoftware.org/isdl.php |
| **SQL Server LocalDB** (тільки для розробки) | входить до складу Visual Studio |

---

## Крок 1 — Публікація застосунку

Відкрийте термінал у корені рішення (де знаходиться `ConstructionCompany.sln`) і виконайте:

```bat
dotnet publish ConstructionCompany\ConstructionCompany.csproj ^
  -c Release ^
  --self-contained true ^
  -r win-x64 ^
  -o installer\publish
```

> **Self-contained** означає, що .NET 8 Runtime вбудовано в дистрибутив.  
> На цільовому ПК не потрібно встановлювати .NET окремо.

---

## Крок 2 — Перевірте структуру папок

Після публікації структура має виглядати так:

```
installer\
  setup.iss           ← скрипт Inno Setup
  publish\            ← результат dotnet publish
    ConstructionCompany.exe
    appsettings.json
    wwwroot\
    ...
  launcher\
    start.bat
    start.vbs
    stop.bat
  Output\             ← буде створено автоматично
```

---

## Крок 3 — Компіляція інсталятора

1. Запустіть **Inno Setup Compiler**
2. `File → Open` → оберіть `installer\setup.iss`
3. `Build → Compile` (або натисніть `F9`)
4. Готовий файл: `installer\Output\ConstructionCompany_Setup.exe`

---

## Що робить інсталятор

| Дія | Деталі |
|---|---|
| Перевірка LocalDB | Зупиняє встановлення, якщо LocalDB відсутній |
| Копіювання файлів | До `C:\Program Files\ConstructionCompany\` |
| Створення ярликів | Робочий стіл + меню «Пуск» |
| Захист даних | Папка `wwwroot\uploads\` не видаляється при деінсталяції |
| Перший запуск | EF Core автоматично створює БД `ConstructionCompanyDb` у LocalDB |

---

## Системні вимоги для кінцевого користувача

- Windows 10 (версія 1809) або Windows 11
- SQL Server LocalDB 2019 або 2022
- ~300 МБ вільного місця на диску
- Будь-який сучасний браузер (Chrome, Edge, Firefox)

---

## Запуск та зупинка

| Дія | Спосіб |
|---|---|
| Запустити | Ярлик «Інформаційна система будівельної компанії» |
| Відкрити у браузері | http://localhost:5000 |
| Зупинити | Меню Пуск → «Зупинити систему» |
| Видалити | Меню Пуск → «Видалити» або «Програми» у Параметрах Windows |

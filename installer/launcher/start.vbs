' ==============================================================
'  start.vbs — запускає start.bat без видимого вікна консолі
'  Використовується ярликами на Робочому столі та в меню «Пуск»
' ==============================================================

Dim oShell, oFS, sDir, sBat

Set oShell = CreateObject("WScript.Shell")
Set oFS    = CreateObject("Scripting.FileSystemObject")

' Визначаємо папку, де знаходиться цей скрипт
sDir = oFS.GetParentFolderName(WScript.ScriptFullName)
sBat = Chr(34) & sDir & "\start.bat" & Chr(34)

' Запускаємо start.bat через cmd з прихованим вікном (WindowStyle = 0)
' bWaitOnReturn = False: не чекаємо завершення
oShell.Run "cmd.exe /c " & sBat, 0, False

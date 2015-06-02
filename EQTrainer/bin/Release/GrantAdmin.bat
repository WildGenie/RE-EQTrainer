@echo off
reg add "HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v %CD%\EQTrainer.exe /d "RUNASADMIN"
reg add "HKCU\Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" /v %CD%\AutoBot.exe /d "RUNASADMIN"
pause
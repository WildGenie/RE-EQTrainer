for /d /r . %%d in (obj,debug) do @if exist "%%d" rd /s/q "%%d"
del /S /F /AH *.suo
rmdir /s /q "AutoBot\bin"
rmdir /s /q "Memory\bin"
rmdir /s /q "Installer\Release"
rmdir /s /q "injectdll2\injectdll2\Release"
cd "EQTrainer\bin\Release"
del /Q Memory.dll
del /S /F *.exe
del /S /F *.pdb
del /S /F *.config
del /S /F *.manifest
del /Q logfile.txt
rmdir /s /q "..\..\..\EQTrainer Setup\EQTrainer Setup"
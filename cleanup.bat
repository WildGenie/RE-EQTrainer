for /d /r . %%d in (obj,debug) do @if exist "%%d" rd /s/q "%%d"
del /S /F /AH *.suo
del /S /F /AH *.htm
rmdir /s /q "AutoBot\bin"
rmdir /s /q "Installer\Release"
rmdir /s /q "injectdll2\injectdll2\Release"
rmdir /s /q ".vs"
cd "EQTrainer\bin\Release"
del /S /F *.exe
del /S /F *.dll
del /S /F *.pdb
del /S /F *.config
del /S /F *.manifest
del /Q logfile.txt
rmdir /s /q "..\..\..\EQTrainer Setup\EQTrainer Setup"
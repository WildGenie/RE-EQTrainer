for /d /r . %%d in (obj,debug) do @if exist "%%d" rd /s/q "%%d"
del /S /F /AH *.suo
rmdir /s /q "AutoBot\bin"
rmdir /s /q "Maps\bin"
rmdir /s /q "Memory\bin"
cd "EQTrainer\bin\Release"
del /Q Memory.dll
del /S /F *.exe
del /S /F *.pdb
del /Q EQTrainer.exe.config
del /Q logfile.txt
@echo off
set /p id="Enter private key file pass: "
"C:\Program Files (x86)\Windows Kits\10\bin\x86\signtool" sign /t http://timestamp.verisign.com/scripts/timestamp.dll /f "C:\Users\jake.CUTTING\Documents\My Private Key.pfx" /p %id% "Debug\setup.exe"
"C:\Program Files (x86)\Windows Kits\10\bin\x86\signtool" sign /t http://timestamp.verisign.com/scripts/timestamp.dll /f "C:\Users\jake.CUTTING\Documents\My Private Key.pfx" /p %id% "Debug\Installer.msi"
pause
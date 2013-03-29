@echo off
del /q /f /s /a:H *.suo
del /q /f /s *.sdf
del /q /f /s *.pdb
del /q /f /s *.ilk
del /q /f /s *.exp
del /q /f /s *.metagen
del /q /f /s *.vshost.exe
del /q /f /s *.vshost.exe.config
del /q /f /s *.manifest
del /q /f /s *.lib
for /d /r . %%d in (obj) do @if exist "%%d" rd /s/q "%%d"
@echo off

SET LOGDIRPATH=%~dp0\build

if not exist "%LOGDIRPATH%" mkdir "%LOGDIRPATH%"

CALL %~dp0\msbuild.bat %~dp0\src\tests.proj ^
    /p:Configuration=Release ^
    /fl /flp:verbosity=normal;LogFile=%LOGDIRPATH%\test.log
@echo off
SET SOLUTIONPATH=%~dp0\src\Utilities.sln
SET LOGDIRPATH=%~dp0\Build
SET LOGFILEPATH=%LOGDIRPATH%\build.log
SET COMMONPARAMETERS=/p:Configuration=Release /M /fl /flp:verbosity=normal;Append;LogFile=%LOGFILEPATH%

if not exist "%LOGDIRPATH%" mkdir "%LOGDIRPATH%"
if exist "%LOGFILEPATH%" del "%LOGFILEPATH%"

REM Restore NuGet packages
CALL %~dp0\msbuild.bat %SOLUTIONPATH% /t:Restore %COMMONPARAMETERS%
if %errorlevel% neq 0 exit /b %errorlevel%

REM Build
CALL %~dp0\msbuild.bat %SOLUTIONPATH% /t:Build %COMMONPARAMETERS%  %*
CALL %~dp0\msbuild.bat %SOLUTIONPATH% /t:Pack %COMMONPARAMETERS% 


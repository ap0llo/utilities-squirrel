@ECHO OFF
SETLOCAL

for /f "usebackq tokens=*" %%i in (`"%programfiles(x86)%\Microsoft Visual Studio\Installer\vswhere" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) do (
  set InstallDir=%%i
)
set MSBuildPath="%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe"

if /I "%APPVEYOR%" == "True" ( 
    SET LOGGER=/logger:C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll
) else (
    SET LOGGER=
)

%MSBuildPath% /clp:verbosity=minimal "%LOGGER%" %*


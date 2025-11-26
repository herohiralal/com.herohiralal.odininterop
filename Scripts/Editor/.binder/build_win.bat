@echo off

SetLocal EnableDelayedExpansion

where /Q cl.exe >nul 2>&1
if errorlevel 1 (
    set __VSCMD_ARG_NO_LOGO=1
    for /f "tokens=*" %%i in ('"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath') do set VS=%%i
    if "!VS!" neq "" (
        call "!VS!\Common7\Tools\vsdevcmd.bat" -arch=x64 -host_arch=x64
    )
)

cl.exe /std:c11 /Fe:../../../Plugins/Editor/OdinInteropBinder.dll /LD ./Binder.c

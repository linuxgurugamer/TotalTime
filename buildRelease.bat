rem garbage line
@echo off
set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
)
echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)

type totaltime.version
set /p VERSION= "Enter version: "



set d=%HOMEDIR\install
if exist %d% goto one
mkdir %d%
:one
set d=%HOMEDIR%\install\Gamedata
if exist %d% goto two
mkdir %d%
:two
set d=%HOMEDIR%\install\Gamedata\TotalTime
if exist %d% goto three
mkdir %d%
:three
set d=%HOMEDIR%\install\Gamedata\TotalTime\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=%HOMEDIR%\install\Gamedata\TotalTime\Textures
if exist %d% goto five
mkdir %d%
:five
set d=%HOMEDIR%\install\Gamedata\TotalTime\PluginData
if exist %d% goto six
mkdir %d%
:six

rem del %HOMEDIR%\install\Gamedata\TotalTime\Textures\*.*


xcopy src\Textures\*.png   %HOMEDIR%\install\GameData\TotalTime\Textures /Y
copy bin\Release\TotalTime.dll %HOMEDIR%\install\Gamedata\TotalTime\Plugins
copy  totaltime.version %HOMEDIR%\install\Gamedata\TotalTime
copy README.md %HOMEDIR%\install\Gamedata\TotalTime
copy ChangeLog.txt %HOMEDIR%\install\Gamedata\TotalTime
pause

%HOMEDRIVE%
cd %HOMEDIR%\install

set FILE="%RELEASEDIR%\TotalTime-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\TotalTime



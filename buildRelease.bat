rem garbage line
@echo off

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"

set VERSIONFILE=totaltime.version
rem The following requires the JQ program, available here: https://stedolan.github.io/jq/download/
c:\local\jq-win64  ".VERSION.MAJOR" %VERSIONFILE% >tmpfile
set /P major=<tmpfile

c:\local\jq-win64  ".VERSION.MINOR"  %VERSIONFILE% >tmpfile
set /P minor=<tmpfile

c:\local\jq-win64  ".VERSION.PATCH"  %VERSIONFILE% >tmpfile
set /P patch=<tmpfile

c:\local\jq-win64  ".VERSION.BUILD"  %VERSIONFILE% >tmpfile
set /P build=<tmpfile
del tmpfile
set VERSION=%major%.%minor%.%patch%
if "%build%" NEQ "0"  set VERSION=%VERSION%.%build%




set d=Gamedata\TotalTime
if exist %d% goto three
mkdir %d%
:three
set d=Gamedata\TotalTime\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=Gamedata\TotalTime\Textures
if exist %d% goto five
mkdir %d%
:five
set d=Gamedata\TotalTime\PluginData
if exist %d% goto six
mkdir %d%
:six

rem del Gamedata\TotalTime\Textures\*.*


xcopy src\Textures\*.png   GameData\TotalTime\Textures /Y
copy bin\Release\TotalTime.dll Gamedata\TotalTime\Plugins
copy  /Y totaltime.version Gamedata\TotalTime
copy /Y ..\MiniAVC.dll Gamedata\TotalTime
copy README.md Gamedata\TotalTime
copy ChangeLog.txt Gamedata\TotalTime
pause

set FILE="%RELEASEDIR%\TotalTime-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\TotalTime



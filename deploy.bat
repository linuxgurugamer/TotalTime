rem first line garbage for some reason
set H=R:\KSP_1.3.1_dev
echo %H%

set d=%H%
if exist %d% goto one
mkdir %d%
:one
set d=%H%\Gamedata
if exist %d% goto two
mkdir %d%
:two
set d=%H%\Gamedata\TotalTime
if exist %d% goto three
mkdir %d%
:three
set d=%H%\Gamedata\TotalTime\Plugins
if exist %d% goto four
mkdir %d%
:four
set d=%H%\Gamedata\TotalTime\Textures
if exist %d% goto five
mkdir %d%
:five

set d=%H%\Gamedata\TotalTime\PluginData
if exist %d% goto six
mkdir %d%
:six


xcopy src\Textures\*.png   %H%\GameData\TotalTime\Textures /Y
copy bin\Debug\TotalTime.dll %H%\Gamedata\TotalTime\Plugins

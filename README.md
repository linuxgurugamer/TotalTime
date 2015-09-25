# TotalTime
A mod to keep track of all the time you spend in the game

This mod will keep track of the following times:

1.  Amount of gametime in the current save game
2.  Amount of gametime in the current install (all saves combined)
3.  The total amount of time played in the game, across all installs
4.  The amount of gametime in the current session (since KSP was started)

Times are not kept for the amount of time when in the initial Main Menu screen.

The toolbar button is only available in the Space Center.  The toolbar button is fairly obvious, it says "Total Time"

Left-clicking on the toolbar button will bring up a small window which shows the times being recorded.  

Right-clicking on the toolbar button will bring up a configuration window.

The configuration options are:

	Save total time for individual saves
	Save total time for this KSP install
	Save total time for all KSP games in external file
	Directory for global count file
	Update interval
	Display on screen
	Display game time
	Display install time
	Display total time
	Display session time

The initial setting do not have the total time for all KSP games enabled, and the directory for 
the file is blank.  If you have multiple installs and wish to keep track of all the times, then 
enable the option and fill in the directory.  There is a button at the bottom which will set 
your home directory as the storage location for the file

The option "Display on screen" controls whether the selected times are displayed on the game screen.  If it is false, 
then you can display the times by left-clicking the toolbar button.

You MUST click the Save button to save the options; if you dismiss the window with the toolbar button,
any changes will be discarded.

The three buttons at the bottom will reset one of the three counters (save, install or global).  Session time cannot
be reset other than by exiting the game and restarting.

Finally, the F2 key is honored, so when you press F2 to hide the UI, all the TotalTime windows and displays are hidden as 
well.  Pressing F2 does not dismiss the configuration window if it is open, it simply hides it.

=======
0.2.0 Addition
A new option is in the Config screen to determine whether time should be kept while 
the game is paused:
	"Include time while paused:"
A new Pause dialog triggered by the Escape key is now available in following scenes:
			Administration Building
			Research and Development
			Astronaut Complex
			Mission Control
			VAB & SPH

This new dialog & pause functionality is controlled by a new config option in the Config screen:
	"Enable Escape key in the Editors and SpaceCenter scenes"
The Pause dialog is only enabled when this is true.

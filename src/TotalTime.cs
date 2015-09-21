using System;
using System.Linq;
using UnityEngine;


namespace TotalTime
{
	[KSPAddon (KSPAddon.Startup.MainMenu, true)]
	public class TotalTime : MonoBehaviour
	{
		public const String TITLE = "Total Time";
		public static Configuration config = new Configuration ();
		public MainMenuGui gui = null;

		public static int secInGame = 0;
		public static int secInInstall = 0;
		public static int secTotal = 0;
		public static int sessionTime = 0;

		public static bool F2 = false;

		public TotalTime ()
		{
		}

		public static void setConfig (ConfigNode config)
		{
			GameDatabase.Instance.GetConfigs ("TOTALTIME").First ().config = config;
		}


		private void readConfig ()
		{
			Log.Debug ("Loading config...");
			var root = GameDatabase.Instance.GetConfigs ("TOTALTIME").First ().config;
			config.parseConfigNode (ref root);
		}

		public void Start ()
		{
			Log.SetTitle ("TotalTime");
			Log.Info ("Start");
			DontDestroyOnLoad (this);
			config.LoadConfiguration ();
#if (DEBUG)
			Log.SetLevel (Log.LEVEL.INFO);
#else
			Log.SetLevel (config.logLevel);
#endif
			// Add a callback to load the data for the game after it is loaded
			GameEvents.onGameStateCreated.Add (CallbackGameStateCreated);

			FileOperations.getData (Configuration.dataLevel.install);
			FileOperations.getData (Configuration.dataLevel.global);

			StartCoroutine (TimeIncrement ());
		}

		private void CallbackGameStateCreated (Game g)
		{
			Log.Info ("CallbackGameStateCreated: " + g.Title);
			strSecInGameTime = "";
			FileOperations.getData (Configuration.dataLevel.game);
		}

		public void Update ()
		{
			if (Input.GetKeyDown (KeyCode.F2))
				F2 = !F2;
			if (gui == null) {
				gui = this.gameObject.AddComponent<MainMenuGui> ();
				gui.UpdateToolbarStock ();
				gui.SetConfigVisible (false);

			}
			if (HighLogic.LoadedScene != GameScenes.MAINMENU) {
				if (MainMenuGui.TT_Button == null)
					GameEvents.onGUIApplicationLauncherReady.Add (gui.OnGUIApplicationLauncherReady);
				gui.OnGUIShowApplicationLauncher ();
			}
		}


		public static string strSecInGameTime, strSecInInstallTime, strSecTotalTime, strSessionTime;
		public const string inGameTitle = "Current save:";
		public const string inInstallTitle = "Install:";
		public const string totalTitle = "KSP Total:";
		public const string sessionTitle = "Session time:";

		private string formatTime (int t)
		{
			int hours, min, msec;
	
			hours = (t / 3600);
			min = (t - hours * 3600) / 60;
			msec = t - (hours * 3600) - (min * 60);

			return(hours.ToString () + ":" + min.ToString ("D2") + ":" + msec.ToString ("D2"));

		}

	//	private  const int interval = 2;

		private System.Collections.IEnumerator TimeIncrement ()
		{
			while (true) {
				if (HighLogic.LoadedScene != GameScenes.MAINMENU) {
					secInGame += config.interval;
					secInInstall += config.interval;
					secTotal += config.interval;
					sessionTime += config.interval;

					strSecInGameTime = formatTime (secInGame);
					strSecInInstallTime = formatTime (secInInstall);
					strSecTotalTime = formatTime (secTotal);
					strSessionTime = formatTime (sessionTime);

					FileOperations.saveData (Configuration.dataLevel.game);
					FileOperations.saveData (Configuration.dataLevel.install);
					FileOperations.saveData (Configuration.dataLevel.global);
				}
				yield return new WaitForSeconds ((float)config.interval);
				
			}
		}

		private const int LEFT = 10;
		private const int TOP = 300;
		private const int WIDTH = 80;
		private const int HEIGHT = 50;
		//private Rect gametimePos = new Rect(10, 200, 80, 50);
		private Rect gametimePos = new Rect (LEFT, TOP, WIDTH, HEIGHT);
		[Persistent]
		private GUIStyle timeLabelStyle;
		[Persistent]
		int timeSize = 10;
		[Persistent]
		float gametimeX = 10;
		[Persistent]
		float gametimeY = 250;

		void DrawOutline (Rect r, string t, int strength, GUIStyle style, Color outColor, Color inColor)
		{
			Color backup = style.normal.textColor;
			style.normal.textColor = outColor;
			for (int i = -strength; i <= strength; i++) {
				GUI.Label (new Rect (r.x - strength, r.y + i, r.width, r.height), t, style);
				GUI.Label (new Rect (r.x + strength, r.y + i, r.width, r.height), t, style);
			}
			for (int i = -strength + 1; i <= strength - 1; i++) {
				GUI.Label (new Rect (r.x + i, r.y - strength, r.width, r.height), t, style);
				GUI.Label (new Rect (r.x + i, r.y + strength, r.width, r.height), t, style);
			}
			style.normal.textColor = inColor;
			GUI.Label (r, t, style);
			style.normal.textColor = backup;
		}

		public void OnGUI ()
		{

			if (HighLogic.LoadedScene != GameScenes.MAINMENU) {
				if (timeLabelStyle == null) {
					timeLabelStyle = new GUIStyle (GUI.skin.label);
					gametimeX = Mathf.Clamp (gametimeX, 0, Screen.width);
					gametimeY = Mathf.Clamp (gametimeY, 0, Screen.height);
					timeLabelStyle.fontSize = timeSize;
				}
					
				if (config.displayOnScreen && !F2) {
					// Only need to get the size of the largest title
					Vector2 size, 
					sizeTitle = timeLabelStyle.CalcSize (new GUIContent (inGameTitle));
					if (config.displayGameTime && config.logGameTime) {
						gametimePos.Set (gametimeX, gametimeY, 200, sizeTitle.y);
						DrawOutline (gametimePos, inGameTitle, 1, timeLabelStyle, Color.black, Color.white);

						size = timeLabelStyle.CalcSize (new GUIContent (strSecInGameTime));
						gametimePos.Set (gametimeX + sizeTitle.x + 5, gametimeY, 200, size.y);
						DrawOutline (gametimePos, strSecInGameTime, 1, timeLabelStyle, Color.black, Color.white);
						gametimePos.Set (gametimePos.xMin, gametimePos.yMin + size.y, 200, size.y);
					}
					if (config.displayInstallTime && config.logInstallTime) {
						gametimePos.Set (gametimeX, gametimeY + timeSize + timeSize / 2, 200, sizeTitle.y);
						DrawOutline (gametimePos, inInstallTitle, 1, timeLabelStyle, Color.black, Color.white);
						
						size = timeLabelStyle.CalcSize (new GUIContent (strSecInInstallTime));
						gametimePos.Set (gametimeX + sizeTitle.x + 5, gametimeY + timeSize + timeSize / 2, 200, size.y);
						DrawOutline (gametimePos, strSecInInstallTime, 1, timeLabelStyle, Color.black, Color.white);
						gametimePos.Set (gametimePos.xMin, gametimePos.yMin + size.y, 200, size.y);
					}
					if (config.displayGlobalTime && config.logGlobalTime) {
						gametimePos.Set (gametimeX, gametimeY + timeSize * 3, 200, sizeTitle.y);
						DrawOutline (gametimePos, totalTitle, 1, timeLabelStyle, Color.black, Color.white);

						size = timeLabelStyle.CalcSize (new GUIContent (strSecTotalTime));
						gametimePos.Set (gametimeX + sizeTitle.x + 5, gametimeY + timeSize * 3, 200, size.y);
						DrawOutline (gametimePos, strSecTotalTime, 1, timeLabelStyle, Color.black, Color.white);
						gametimePos.Set (gametimePos.xMin, gametimePos.yMin + size.y, 200, size.y);
					}

					if (config.displaySessionTime ) {
						gametimePos.Set (gametimeX, gametimeY + timeSize * 4 + timeSize / 2, 200, sizeTitle.y);
						DrawOutline (gametimePos, sessionTitle, 1, timeLabelStyle, Color.black, Color.white);

						size = timeLabelStyle.CalcSize (new GUIContent (strSessionTime));
						gametimePos.Set (gametimeX + sizeTitle.x + 5, gametimeY + timeSize * 4 + timeSize / 2, 200, size.y);
						DrawOutline (gametimePos, strSessionTime, 1, timeLabelStyle, Color.black, Color.white);
						gametimePos.Set (gametimePos.xMin, gametimePos.yMin + size.y, 200, size.y);
					}

				
				}
			}

		}
	
	}
}


using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using KSP.UI.Screens;

namespace TotalTime
{
	[KSPAddon (KSPAddon.Startup.SpaceCentre, true)]
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

		public static bool paused = false;
		public static bool subscene = false;
		public static GameScenes lastScene = GameScenes.CREDITS;

		public TotalTime ()
		{
		}

		public static void setConfig (ConfigNode config)
		{
			Log.Info ("setConfig");
            if (config != null)
            {
                if (GameDatabase.Instance.GetConfigs("TotalTime") != null)
                {
                    Log.Info("Totaltime config found");
                    if (GameDatabase.Instance.GetConfigs("TotalTime").Length > 0)
                        GameDatabase.Instance.GetConfigs("TotalTime").First().config = config;
                }
            }
			Log.Info ("setConfig done");
		}


		private void readConfig ()
		{
			Log.Debug ("Loading config...");
			var root = GameDatabase.Instance.GetConfigs ("TotalTime").First ().config;
			config.parseConfigNode (ref root);
//			MainMenuGui.infoDisplayVisible = TotalTime.config.displayInWindow;

		}

	//	void OnApplicationPause(bool pauseStatus)
	//	{
	//		Log.Info ("OnApplicationPause: " + pauseStatus.ToString ());
	//	}
		public void OnPause()
		{
			paused = true;
//			if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
//				if (subscene)
//					InputLockManager.SetControlLock(ControlTypes.All, "lockID");
				//else InputLockManager.RemoveControlLock("lockID");
//			}
		}
		public void OnResume()
		{
			paused = false;
//			if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
//				if (subscene)
//					InputLockManager.SetControlLock(ControlTypes.All, "lockID");
//				 InputLockManager.RemoveControlLock("lockID");
//			}
		}
		private void CallbackLevelWasLoaded(Scene scene, LoadSceneMode mode)
        {
			if (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.EDITOR) {
				paused = false;
//				InputLockManager.RemoveControlLock("lockID");
			}
			lastScene = HighLogic.LoadedScene;
		}

		private void CallbackAdminFacility()
		{
#if false
            if (!config.includePauseTime) {
				subscene = !subscene;
				if (!subscene)
					paused = false;
			}
#endif
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

            FileOperations.MoveCfgToDataDir();

            if (config.logInstallTime)
				FileOperations.getData (Configuration.dataLevel.install);
			if (config.logGameTime)
				FileOperations.getData (Configuration.dataLevel.global);

			GameEvents.onGamePause.Add (OnPause);
			GameEvents.onGameUnpause.Add (OnResume);
			//GameEvents.onLevelWasLoaded.Add (CallbackLevelWasLoaded);

			GameEvents.onGUIAdministrationFacilitySpawn.Add (CallbackAdminFacility);
			GameEvents.onGUIAdministrationFacilityDespawn.Add (CallbackAdminFacility);
			GameEvents.onGUIMissionControlSpawn.Add (CallbackAdminFacility);
			GameEvents.onGUIMissionControlDespawn.Add (CallbackAdminFacility);
			GameEvents.onGUIRnDComplexSpawn.Add (CallbackAdminFacility);
			GameEvents.onGUIRnDComplexDespawn.Add (CallbackAdminFacility);
			GameEvents.onGUIAstronautComplexSpawn.Add(CallbackAdminFacility);
			GameEvents.onGUIAstronautComplexDespawn.Add(CallbackAdminFacility);

            gui.OnGUIShowApplicationLauncher();

            StartCoroutine (TimeIncrement ());
		}

		private void OnDestroy()
		{
			GameEvents.onGamePause.Remove(OnPause);
			GameEvents.onGameUnpause.Remove(OnResume); 
			GameEvents.onGameStateCreated.Remove (CallbackGameStateCreated);
			//GameEvents.onLevelWasLoaded.Remove (CallbackLevelWasLoaded);

			GameEvents.onGUIAdministrationFacilitySpawn.Remove (CallbackAdminFacility);
			GameEvents.onGUIAdministrationFacilityDespawn.Remove (CallbackAdminFacility);
			GameEvents.onGUIMissionControlSpawn.Remove (CallbackAdminFacility);
			GameEvents.onGUIMissionControlDespawn.Remove (CallbackAdminFacility);
			GameEvents.onGUIRnDComplexSpawn.Remove (CallbackAdminFacility);
			GameEvents.onGUIRnDComplexDespawn.Remove (CallbackAdminFacility);
			GameEvents.onGUIAstronautComplexSpawn.Remove(CallbackAdminFacility);
			GameEvents.onGUIAstronautComplexDespawn.Remove(CallbackAdminFacility);
		}

        void OnEnable()
        {
            //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
            SceneManager.sceneLoaded += CallbackLevelWasLoaded;
        }

        void OnDisable()
        {
            //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= CallbackLevelWasLoaded;
        }
        private void CallbackGameStateCreated (Game g)
		{
			Log.Info ("CallbackGameStateCreated: " + g.Title);
			strSecInGameTime = "";
			FileOperations.getData (Configuration.dataLevel.game);
		}

		public void Update ()
		{
			if ((Input.GetKeyDown (KeyCode.Escape) && config.enableEscapePause) && (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.EDITOR)) {
					paused = !paused;
				if (subscene) {
					if (paused)
						InputLockManager.SetControlLock (ControlTypes.All, "TotalTime");
					else
						InputLockManager.RemoveControlLock ("TotalTime");
				}

			}
			if (Input.GetKeyDown (KeyCode.F2))
				F2 = !F2;
			if (gui == null) {
				gui = this.gameObject.AddComponent<MainMenuGui> ();
				gui.UpdateToolbarStock ();
				gui.SetConfigVisible (false);

			}

			if (HighLogic.LoadedScene != GameScenes.MAINMENU) {
#if false
                if (MainMenuGui.TT_Button == null)
					GameEvents.onGUIApplicationLauncherReady.Add (gui.OnGUIApplicationLauncherReady);
#endif
				
			}
		}


		public static string strSecInGameTime, strSecInInstallTime, strSecTotalTime, strSessionTime = formatTime(sessionTime);
		public const string inGameTitle = "Current save:";
		public const string inInstallTitle = "Install:";
		public const string totalTitle = "KSP Total:";
		public const string sessionTitle = "Session time:";

		public static string formatTime (int t)
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
				//if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
					//if (Planetarium.GetUniversalTime() - lasttime 
				Log.Info("LoadedScene: " + HighLogic.LoadedScene.ToString());
					Log.Info("TimeIncrement, time: " + Planetarium.GetUniversalTime().ToString());
					Log.Info("Planetarium.Pause: " + Planetarium.Pause.ToString());
				//}
				if (HighLogic.LoadedScene == GameScenes.SPACECENTER && !Planetarium.Pause && paused)
					paused = false;
				if (HighLogic.LoadedScene != GameScenes.MAINMENU && (!paused || config.includePauseTime))
				{
					secInGame += config.interval;
					secInInstall += config.interval;
					secTotal += config.interval;
					sessionTime += config.interval;

					strSecInGameTime = formatTime (secInGame);
					strSecInInstallTime = formatTime (secInInstall);
					strSecTotalTime = formatTime (secTotal);
					strSessionTime = formatTime (sessionTime);

					if (config.logGameTime)
						FileOperations.saveData (Configuration.dataLevel.game);
					if (config.logInstallTime)
						FileOperations.saveData (Configuration.dataLevel.install);
					if (config.logGlobalTime)
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


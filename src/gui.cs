using System;
using System.IO;
using KSP.UI.Screens;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using UnityEngine;

namespace TotalTime
{
	public class MainMenuGui : MonoBehaviour
	{

		private static Texture2D TT_button_img = new Texture2D (38, 38, TextureFormat.ARGB32, false);
		private static bool TT_Texture_Load = false;
		public static ApplicationLauncherButton TT_Button = null;

		public static bool configDisplayActive = false;
		public static bool infoDisplayActive = false;

		private bool cfgWinData = false;
		private /* volatile*/ bool configVisible = false;
//		public  static bool infoDisplayVisible = false;
		//private bool defaultsLoaded = false;
		private static bool appLaucherHidden = true;

		private const int WIDTH = 400;
		private const int HEIGHT = 300;
		private Rect configBounds = new Rect (Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

		private const int INFOWIDTH = 175;
		private const int INFOHEIGHT = 50;
		private Rect infoBounds = new Rect (Screen.width / 4 - INFOWIDTH / 2, Screen.height / 4 - INFOHEIGHT / 2, INFOWIDTH, INFOHEIGHT);

		private  static int PAUSEWIDTH = Screen.width / 2;
		private  static int PAUSEHEIGHT = Screen.height / 2;
		private Rect pauseBounds = new Rect (Screen.width / 2 - PAUSEWIDTH / 2, 50, PAUSEWIDTH, PAUSEHEIGHT);
		GUIStyle largeFont = new GUIStyle ();
		GUIStyle b = new GUIStyle (); //(GUI.skin.window);

		#if false
		public MainMenuGui ()
		{
		}

		private void Start ()
		{
		}
		#endif
		public void UpdateToolbarStock ()
		{

			// Create the button in the KSP AppLauncher

			if (!TT_Texture_Load) {

				if (GameDatabase.Instance.ExistsTexture (FileOperations.TEXTURE_DIR + "TotalTime-38")) {
					TT_button_img = GameDatabase.Instance.GetTexture (FileOperations.TEXTURE_DIR + "TotalTime-38", false);
				}

				TT_Texture_Load = true;
			}
			if (TT_Button == null) {

				TT_Button = ApplicationLauncher.Instance.AddModApplication (GUIToggleToolbar, GUIToggleToolbar,
					null, null,
					null, null,
					ApplicationLauncher.AppScenes.SPACECENTER,
					TT_button_img);
				//stockToolBarcreated = true;
			}
		}

		public void SetConfigVisible (bool visible)
		{
			configVisible = visible;
		}

//		public void SetInfoDisplayVisible (bool visible)
//		{
//			infoDisplayVisible = visible;
//		}

		public void GUIToggleToolbar ()
		{
			//GUIToggle (true);
			Log.Info("GUIToggleToolbar");
			//stockToolBarcreated = true;
			if (!Input.GetMouseButtonUp (1)) {
				//onRightButtonStockClick ();
				configDisplayActive = !configDisplayActive;
				if (configDisplayActive) {
					SetConfigVisible (true);
					cfgWinData = false;
				} else {

					SetConfigVisible (false);
					Log.Info("displayInWindow 1: " + TotalTime.config.displayInWindow.ToString() );
//					SetInfoDisplayVisible(TotalTime.config.displayInWindow);
					//UpdateToolbarStock ();
				}
			}
			#if false
			if (!Input.GetMouseButtonUp (1)) {
				//onLeftButtonClick();
				infoDisplayActive = !infoDisplayActive;
				if (infoDisplayActive) {
					SetInfoDisplayVisible (true);
				} else {
					SetInfoDisplayVisible (false);

					//UpdateToolbarStock ();
				}
			}
			#else
//			if (!configVisible)
//			{
//				SetInfoDisplayVisible(TotalTime.config.displayInWindow);
//				Log.Info("displayInWindow: " + TotalTime.config.displayInWindow.ToString() );
//			}
			#endif
		}

		public void GUIToggleConfig ()
		{
			
			configDisplayActive = !configDisplayActive;
			if (configDisplayActive) {
				SetConfigVisible (true);
				cfgWinData = false;
			} else {
//				SetInfoDisplayVisible(TotalTime.config.displayInWindow);
				SetConfigVisible (false);
			}

		}

		public void OnGUIShowApplicationLauncher ()
		{
			if (appLaucherHidden) {
				appLaucherHidden = false;
				//if (TT_Button != null)
				//	UpdateToolbarStock ();
			}

		}

		public void OnGUIApplicationLauncherReady ()
		{
			UpdateToolbarStock ();
		}

		public void OnGUI ()
		{
			if ((TotalTime.subscene || HighLogic.LoadedScene == GameScenes.EDITOR) && TotalTime.paused && TotalTime.lastScene != GameScenes.FLIGHT) {
				
				b.normal.background = MakeTex (2, 2, Color.gray);
				this.pauseBounds = GUILayout.Window (GetInstanceID () + 2, pauseBounds, PauseWindow, "", b);
			}

			if (!TotalTime.F2) {
				if (this.configVisible) {
					try {
						this.configBounds = GUILayout.Window (GetInstanceID (), configBounds, ConfigWindow, TotalTime.TITLE, HighLogic.Skin.window);
				
					} catch (Exception) {
					}
				}

				if (TotalTime.config.displayInWindow && (
					(TotalTime.config.displayGameTime && TotalTime.config.logGameTime) ||
					(TotalTime.config.displayInstallTime && TotalTime.config.logInstallTime) ||
					(TotalTime.config.displayGlobalTime && TotalTime.config.logGlobalTime) ||
					(TotalTime.config.displaySessionTime) )) {
					try {
						this.infoBounds = GUILayout.Window (GetInstanceID () + 1, infoBounds, InfoWindow, TotalTime.TITLE, HighLogic.Skin.window);
					} catch (Exception) {
					}
				}
			}
		}


		private Texture2D MakeTex (int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];

			for (int i = 0; i < pix.Length; i++)
				pix [i] = col;

			Texture2D result = new Texture2D (width, height);
			result.SetPixels (pix);
			result.Apply ();

			return result;
		}

		private void PauseWindow (int id)
		{
			int h = 150;

			largeFont.fontSize = 22;

			GUI.enabled = true;

			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			GUILayout.Label ("Game Paused", largeFont, GUILayout.Height (h));
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			GUILayout.Label ("Press Escape to continue, or click the button below", largeFont, GUILayout.Height (h));
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Resume Game", GUILayout.Height (h), GUILayout.Width (300))) {
				TotalTime.paused = false;
				InputLockManager.RemoveControlLock ("TotalTime");
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();

			GUILayout.EndVertical ();


		}

		private void InfoWindow (int id)
		{
			//SetInfoDisplayVisible (true);
			GUI.enabled = true;

			GUILayout.BeginVertical ();
			//GUILayout.BeginHorizontal ();
			//GUILayout.Label ("");
			//GUILayout.EndHorizontal ();
			if (TotalTime.config.displayGameTime && TotalTime.config.logGameTime) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (TotalTime.inGameTitle);
				GUILayout.FlexibleSpace ();
				GUILayout.Label (TotalTime.strSecInGameTime);
				GUILayout.EndHorizontal ();
			}
			if (TotalTime.config.displayInstallTime && TotalTime.config.logInstallTime) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (TotalTime.inInstallTitle);
				GUILayout.FlexibleSpace ();
				GUILayout.Label (TotalTime.strSecInInstallTime);
				GUILayout.EndHorizontal ();
			}
			if (TotalTime.config.displayGlobalTime && TotalTime.config.logGlobalTime) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (TotalTime.totalTitle);
				GUILayout.FlexibleSpace ();
				GUILayout.Label (TotalTime.strSecTotalTime);
				GUILayout.EndHorizontal ();
			}

			if (TotalTime.config.displaySessionTime) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (TotalTime.sessionTitle);
				GUILayout.FlexibleSpace ();
				GUILayout.Label (TotalTime.strSessionTime);
				GUILayout.EndHorizontal ();
			}

			GUI.DragWindow ();
			GUILayout.EndVertical ();
		}

		string strtotalTimeDataPath, strinterval;
		bool boollogGameTime, boollogInstallTime, boollogGlobalTime;
		bool booldisplayGameTime, booldisplayInstallTime, booldisplayGlobalTime, booldisplayOnScreen, booldisplayInWindow, booldisplaySessionTime, boolincludePauseTime, boolenableEscapePause;

		void setGuiVars (ref Configuration config)
		{
			strtotalTimeDataPath = config.totalTimeDataPath;
			strinterval = config.interval.ToString ();
			boollogGameTime = config.logGameTime;
			boollogInstallTime = config.logInstallTime;
			boollogGlobalTime = config.logGlobalTime;

			booldisplayGameTime = config.displayGameTime;
			booldisplayInstallTime = config.displayInstallTime;
			booldisplayGlobalTime = config.displayGlobalTime;
			booldisplayOnScreen = config.displayOnScreen;
			booldisplayInWindow = config.displayInWindow;
			booldisplaySessionTime = config.displaySessionTime;
			boolincludePauseTime = config.includePauseTime;
			boolenableEscapePause = config.enableEscapePause;
		}

		private void ConfigWindow (int id)
		{
			if (cfgWinData == false) {
				cfgWinData = true;

				setGuiVars (ref TotalTime.config);

			}

			//SetConfigVisible (true);
			GUI.enabled = true;

			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.EndHorizontal ();

			//GUILayout.BeginArea (new Rect (10, 50, 375, 500));

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Save total time for individual saves:");
			GUILayout.FlexibleSpace ();
			boollogGameTime = GUILayout.Toggle (boollogGameTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Save total time for this KSP install:");
			GUILayout.FlexibleSpace ();
			boollogInstallTime = GUILayout.Toggle (boollogInstallTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Save total time for all KSP games in external file:");
			GUILayout.FlexibleSpace ();
			boollogGlobalTime = GUILayout.Toggle (boollogGlobalTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Directory for global count file:");
			GUILayout.FlexibleSpace ();
			strtotalTimeDataPath = GUILayout.TextField (strtotalTimeDataPath, GUILayout.MinWidth (30F), GUILayout.MaxWidth (300F));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Update interval:");
			GUILayout.FlexibleSpace ();
			strinterval = GUILayout.TextField (strinterval, GUILayout.MinWidth (30F), GUILayout.MaxWidth (30F));
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Include time while paused:");
			GUILayout.FlexibleSpace ();
			boolincludePauseTime = GUILayout.Toggle (boolincludePauseTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Enable Escape key in the Editors and SpaceCenter scenes:");
			GUILayout.FlexibleSpace ();
			boolenableEscapePause = GUILayout.Toggle (boolenableEscapePause, "");
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Display on screen:");
			GUILayout.FlexibleSpace ();
			booldisplayOnScreen = GUILayout.Toggle (booldisplayOnScreen, "");
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Display in window:");
			GUILayout.FlexibleSpace ();
			booldisplayInWindow = GUILayout.Toggle (booldisplayInWindow, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Display game time:");
			GUILayout.FlexibleSpace ();
			booldisplayGameTime = GUILayout.Toggle (booldisplayGameTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Display install time:");
			GUILayout.FlexibleSpace ();
			booldisplayInstallTime = GUILayout.Toggle (booldisplayInstallTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Display total time:");
			GUILayout.FlexibleSpace ();
			booldisplayGlobalTime = GUILayout.Toggle (booldisplayGlobalTime, "");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Display session time:");
			GUILayout.FlexibleSpace ();
			booldisplaySessionTime = GUILayout.Toggle (booldisplaySessionTime, "");
			GUILayout.EndHorizontal ();



			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Defaults", GUILayout.Width (125.0f))) {
				Configuration c = new Configuration ();
				//TotalTime.config.setDefaults ();
				//cfgWinData = false;
				setGuiVars (ref c);
				return;
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Save", GUILayout.Width (125.0f))) {
				//writeConfig (newconfig);
				//	bool hasWriteAccess = true;
				#if false
				string tmpFileName = strtotalTimeDataPath + "/tmp.tmp";
				FileInfo testfileInfo = new FileInfo (tmpFileName);

				Log.Info ("testing write access to: " + tmpFileName);

				try {
					Log.Info("testfileInfo.Directory.Create");
					testfileInfo.Directory.Create();
				} catch (Exception SystemSecurityException) {
					//hasWriteAccess = false;   
					Log.Info("SystemSecurityException");
					Log.Info(SystemSecurityException.ToString());
					TotalTime.config.totalTimeDataPath = "";
					boollogGlobalTime = false;
				}

				if (File.Exists (tmpFileName)) {
					testfileInfo.Delete ();
					TotalTime.config.totalTimeDataPath = strtotalTimeDataPath;
				} else {
					TotalTime.config.totalTimeDataPath = "";
					boollogGlobalTime = false;
				}
				#else
				TotalTime.config.totalTimeDataPath = strtotalTimeDataPath;
				#endif
				if (TotalTime.config.totalTimeDataPath == "")
					booldisplayGlobalTime = false;
				TotalTime.config.logGameTime = boollogGameTime;
				if (TotalTime.config.logInstallTime == false && boollogInstallTime)
				{
					FileOperations.getData (Configuration.dataLevel.install);
				}
				TotalTime.config.logInstallTime = boollogInstallTime;
				if (TotalTime.config.logGlobalTime == false && boollogGameTime)
				{
					FileOperations.getData (Configuration.dataLevel.global);
				}
				TotalTime.config.logGlobalTime = boollogGlobalTime;

				TotalTime.config.displayGameTime = booldisplayGameTime;
				TotalTime.config.displayInstallTime = booldisplayInstallTime;
				TotalTime.config.displayGlobalTime = booldisplayGlobalTime;
				TotalTime.config.displayOnScreen = booldisplayOnScreen;

				TotalTime.config.displayInWindow = booldisplayInWindow;

				TotalTime.config.displaySessionTime = booldisplaySessionTime;
				TotalTime.config.includePauseTime = boolincludePauseTime;
				TotalTime.config.enableEscapePause = boolenableEscapePause;

				try {
					TotalTime.config.interval = Convert.ToUInt16 (strinterval);
				} catch (Exception) {
				} 
				GUIToggleConfig ();
				TotalTime.config.SaveConfiguration ();

			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Cancel", GUILayout.Width (125.0f))) {
				GUIToggleConfig ();
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Set global file counter file in home directory", GUILayout.Width (300.0f))) {
				strtotalTimeDataPath = FileOperations.GetHomeDir ();
				strtotalTimeDataPath = strtotalTimeDataPath.Replace ("\\", "/");

				int i = TotalTime.secTotal;
				FileOperations.getData (Configuration.dataLevel.global);
				TotalTime.secTotal += i;

			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Reset Save counter", GUILayout.Width (150.0f))) {
				TotalTime.secInGame = 0;
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Reset Install counter", GUILayout.Width (150.0f))) {
				TotalTime.secInInstall = 0;
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Reset Global counter", GUILayout.Width (150.0f))) {
				TotalTime.secTotal = 0;
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			GUI.DragWindow ();
		}

	}
}


using System;
using System.IO;

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
		private bool infoDisplayVisible = false;
		//private bool defaultsLoaded = false;
		private static bool appLaucherHidden = true;

		private const int WIDTH = 400;
		private const int HEIGHT = 300;
		private Rect configBounds = new Rect (Screen.width / 2 - WIDTH / 2, Screen.height / 2 - HEIGHT / 2, WIDTH, HEIGHT);

		private const int INFOWIDTH = 175;
		private const int INFOHEIGHT = 50;
		private Rect infoBounds = new Rect (Screen.width / 4 - INFOWIDTH / 2, Screen.height / 4 - INFOHEIGHT / 2, INFOWIDTH, INFOHEIGHT);



		public MainMenuGui ()
		{
		}

		private void Start ()
		{
		}

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

		public void SetInfoDisplayVisible (bool visible)
		{
			infoDisplayVisible = visible;
		}

		public void GUIToggleToolbar ()
		{
			//GUIToggle (true);

			//stockToolBarcreated = true;
			if (Input.GetMouseButtonUp (1)) {
				//onRightButtonStockClick ();
				configDisplayActive = !configDisplayActive;
				if (configDisplayActive) {
					SetConfigVisible (true);
					cfgWinData = false;
				} else {

					SetConfigVisible (false);

					//UpdateToolbarStock ();
				}
			}
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
		}

		public void GUIToggleConfig ()
		{
			
			configDisplayActive = !configDisplayActive;
			if (configDisplayActive) {
				SetConfigVisible (true);
				cfgWinData = false;
			} else {

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
			if (!TotalTime.F2) {
				if (this.configVisible) {
					try {
						this.configBounds = GUILayout.Window (this.GetInstanceID (), this.configBounds, this.ConfigWindow, TotalTime.TITLE, HighLogic.Skin.window);
				
					} catch (Exception) {
					}
				}

				if (this.infoDisplayVisible) {
					try {
						this.infoBounds = GUILayout.Window (this.GetInstanceID () + 1, this.infoBounds, this.InfoWindow, TotalTime.TITLE, HighLogic.Skin.window);
					} catch (Exception) {
					}
				}
			}
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
		bool booldisplayGameTime, booldisplayInstallTime, booldisplayGlobalTime, booldisplayOnScreen, booldisplaySessionTime;

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
			booldisplaySessionTime = config.displaySessionTime;
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
			GUILayout.Label ("Display on screen:");
			GUILayout.FlexibleSpace ();
			booldisplayOnScreen = GUILayout.Toggle (booldisplayOnScreen, "");
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
				TotalTime.config.logInstallTime = boollogInstallTime;
				TotalTime.config.logGlobalTime = boollogGlobalTime;

				TotalTime.config.displayGameTime = booldisplayGameTime;
				TotalTime.config.displayInstallTime = booldisplayInstallTime;
				TotalTime.config.displayGlobalTime = booldisplayGlobalTime;
				TotalTime.config.displayOnScreen = booldisplayOnScreen;
				TotalTime.config.displaySessionTime = booldisplaySessionTime;
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


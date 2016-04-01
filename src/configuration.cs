//using UnityEngine;
using System;

//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


namespace TotalTime
{
	public class Configuration
	{
		private static ConfigNode configFile = null;
		private static ConfigNode configFileNode = null;

		[Persistent] 
		public Log.LEVEL logLevel { get; set; }

		public string totalTimeDataPath;

		public enum dataLevel
		{
			game,
			install,
			global
		}


		public bool logGameTime, logInstallTime, logGlobalTime;
		public bool displayOnScreen;
		public bool displayGameTime, displayInstallTime, displayGlobalTime, displaySessionTime;
		public int interval;
		public bool includePauseTime;
		public bool enableEscapePause;
		public bool displayInWindow;

		public Configuration ()
		{
#if (DEBUG)
			logLevel = Log.LEVEL.INFO;
#else
			logLevel = Log.LEVEL.WARNING;
#endif
			setDefaults ();
			Log.Info ("Configuration - Setting default config");
		}


		public void setDefaults ()
		{
			totalTimeDataPath = "";
			logGameTime = true;
			logInstallTime = true;
			logGlobalTime = false;
			displayGameTime = true;
			displayInstallTime = true;
			displayGlobalTime = false;
			displaySessionTime = true;
			displayOnScreen = true;
			displayInWindow = false;
			includePauseTime = false;
			enableEscapePause = true;
			interval = 5;
		}

		public void LoadConfiguration ()
		{
			setDefaults ();
			configFile = ConfigNode.Load (FileOperations.TT_CFG_FILE);

			if (configFile != null) {
				configFileNode = configFile.GetNode (FileOperations.TT_NODENAME);
				if (configFileNode != null) {
					parseConfigNode (ref configFileNode);
				}
			}
		}

		public void SaveConfiguration ()
		{
			Log.Info ("SaveConfiguration");
			ConfigNode root = new ConfigNode ();
			ConfigNode top = new ConfigNode (FileOperations.TT_NODENAME);
			root.SetNode (FileOperations.TT_NODENAME, top, true);

			top.SetValue ("totalTimeDataPath", totalTimeDataPath.ToString (), true);

			top.SetValue ("logGameTime", logGameTime.ToString (), true);
			top.SetValue ("logInstallTime", logInstallTime.ToString (), true);
			top.SetValue ("logGlobalTime", logGlobalTime.ToString (), true);
			top.SetValue ("updateInterval", interval.ToString (), true);

			top.SetValue ("displayGameTime", displayGameTime.ToString (), true);
			top.SetValue ("displayInstallTime", displayInstallTime.ToString (), true);
			top.SetValue ("displayGlobalTime", displayGlobalTime.ToString (), true);
			top.SetValue ("displaySessionTime", displaySessionTime.ToString (), true);
			top.SetValue ("includePauseTime", includePauseTime.ToString (), true);
			top.SetValue ("enableEscapePause", enableEscapePause.ToString (), true);

			top.SetValue ("displayOnScreen", displayOnScreen.ToString (), true);
			top.SetValue ("displayInWindow", displayInWindow.ToString (), true);

			root.Save (FileOperations.TT_CFG_FILE);
			TotalTime.setConfig (root);
		}

		public void parseConfigNode (ref ConfigNode root)
		{
			
			try {
				totalTimeDataPath = root.GetValue ("totalTimeDataPath");
			} catch {
			}
			try {
				logGameTime = Boolean.Parse (root.GetValue ("logGameTime"));
			} catch {
			}
			try {
				logInstallTime = Boolean.Parse (root.GetValue ("logInstallTime"));
			} catch {
			}
			try {
				logGlobalTime = Boolean.Parse (root.GetValue ("logGlobalTime"));
			} catch {
			}

			try {
				interval = Convert.ToUInt16 (root.GetValue ("updateInterval"));
			} catch (Exception) {
			} 
			if (interval <= 0)
				interval = 2;
			
			try {
				displayOnScreen = Boolean.Parse (root.GetValue ("displayOnScreen"));
			} catch {
			}

			try {
				displayInWindow = Boolean.Parse (root.GetValue ("displayInWindow"));
			} catch {
			}

			try {
				displayGameTime = Boolean.Parse (root.GetValue ("displayGameTime"));
			} catch {
			}

			try {
				includePauseTime = Boolean.Parse (root.GetValue ("includePauseTime"));
			} catch {
			}
				
			try {
				enableEscapePause = Boolean.Parse (root.GetValue ("enableEscapePause"));
			} catch {
			}


			try {
				displayInstallTime = Boolean.Parse (root.GetValue ("displayInstallTime"));
			} catch {
			}
			try {
				displayGlobalTime = Boolean.Parse (root.GetValue ("displayGlobalTime"));
			} catch {
			}
			try {
				displaySessionTime = Boolean.Parse (root.GetValue ("displaySessionTime"));
			} catch {
			}

		}
	}
}

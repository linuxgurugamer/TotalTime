
// just uncomment this line to restrict file access to KSP installation folder
#define _UNLIMITED_FILE_ACCESS

using System;
using UnityEngine;


namespace TotalTime
{
	public  class FileOperations
	{
		public static readonly String ROOT_PATH = KSPUtil.ApplicationRootPath;
		private static readonly String CONFIG_BASE_FOLDER = ROOT_PATH + "GameData/";
		public static String TT_BASE_FOLDER = CONFIG_BASE_FOLDER + "TotalTime/";
		public static string TEXTURE_DIR = "TotalTime/" + "Textures/";
		private  string SAVE_PATH = ROOT_PATH + "saves/" + HighLogic.SaveFolder;
		public static string TT_DATAFILE = "totaltime.dat";
		public static String TT_NODENAME = "TotalTime";
		public static String TT_CFG_FILE = FileOperations.TT_BASE_FOLDER + "TotalTime.cfg";


//		private static ConfigNode configFile = null;
//		private static ConfigNode configFileNode = null;
//		public Configuration config;


#if (!_UNLIMITED_FILE_ACCESS)
		public static bool InsideApplicationRootPath(String path)
		{
		if (path == null) return false;
		try
		{
		String fullpath = Path.GetFullPath(path);
		return fullpath.StartsWith(Path.GetFullPath(ROOT_PATH));
		}
		catch
		{
		return false;
		}
		}
#endif

		public static bool ValidPathForWriteOperation(String path)
		{
#if (_UNLIMITED_FILE_ACCESS)
			return true;
#else
			String fullpath = Path.GetFullPath(path);
			return InsideApplicationRootPath(fullpath);
#endif
		}

		public static String GetHomeDir()
		{
			if (Application.platform == RuntimePlatform.WindowsPlayer)
				return Environment.GetEnvironmentVariable("USERPROFILE");
			return Environment.GetEnvironmentVariable("HOME");
		}

		private static string getDataFile(Configuration.dataLevel type)
		{
			switch (type) {
			case Configuration.dataLevel.game:
				// This happens when this is called before a save is loaded or created
				if (HighLogic.SaveFolder == "DestructiblesTest")
					return "";
				return(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/" + TT_DATAFILE);

				case Configuration.dataLevel.install:
					return(CONFIG_BASE_FOLDER + TT_DATAFILE);

			case Configuration.dataLevel.global:
				if (TotalTime.config.totalTimeDataPath == "")
					return "";
					return(TotalTime.config.totalTimeDataPath + "/" + FileOperations.TT_DATAFILE);
			}
			return "";
		}


		public static  bool getData(Configuration.dataLevel type)
		{
			Log.Info ("getData dataLevel: " + type.ToString ());
			string path = getDataFile(type);
			Log.Info ("getData path: " + path);
			if (path == "" || !System.IO.File.Exists (path)) {
				switch (type) {
				case Configuration.dataLevel.game:
					TotalTime.secInGame = 0;
					break;

				case Configuration.dataLevel.install:
					TotalTime.secInInstall = 0;
					break;

				case Configuration.dataLevel.global:
					TotalTime.secTotal = 0;
					break;
				}
				Log.Info ("path/file not found");
				return false;
			}

			ConfigNode root = new ConfigNode ();
			root = ConfigNode.Load (path);
			ConfigNode top = root.GetNode (FileOperations.TT_NODENAME);
			if (top != null) {
				switch (type) {
				case Configuration.dataLevel.game:
					TotalTime.secInGame = int.Parse (SafeLoad (top.GetValue ("secInGame"), TotalTime.secInGame.ToString ()));
					Log.Info ("getData: dataLevel.game: " + TotalTime.secInGame);
					break;

				case Configuration.dataLevel.install:
					TotalTime.secInInstall = int.Parse (SafeLoad (top.GetValue ("secInInstall"), TotalTime.secInInstall.ToString ()));
					Log.Info ("getData: dataLevel.install: " + TotalTime.secInInstall);
					break;

				case Configuration.dataLevel.global:
					TotalTime.secTotal = int.Parse (SafeLoad (top.GetValue ("secTotal"), TotalTime.secTotal.ToString ()));
					Log.Info ("getData: dataLevel.global: " + TotalTime.secTotal);
					break;
				}
			}
			return false;
		}

		public static bool saveData(Configuration.dataLevel type)
		{
			Log.Info ("saveData dataLevel: " + type.ToString ());
			string path = getDataFile(type);
			Log.Info ("saveData: " + path);
			if (path == "")
				return false;
			
			ConfigNode root = new ConfigNode ();
			ConfigNode top = new ConfigNode (FileOperations.TT_NODENAME);
			root.SetNode (FileOperations.TT_NODENAME, top, true);

			switch (type) {
			case Configuration.dataLevel.game:
				top.SetValue ("secInGame", TotalTime.secInGame.ToString (), true);
				break;
			case Configuration.dataLevel.install:
				top.SetValue ("secInInstall", TotalTime.secInInstall.ToString (), true);
				break;
			case Configuration.dataLevel.global:
				top.SetValue ("secTotal", TotalTime.secTotal.ToString (), true);
				break;
			}


			root.Save (path);
			return false;
		}


		//
		// The following functions are used when loading data from the config file
		// They make sure that if a value is missing, that the old value will be used.
		// 
		static string SafeLoad (string value, string oldvalue)
		{
			if (value == null)
				return oldvalue;
			return value;
		}
		#if false
		static string SafeLoad (string value, bool oldvalue)
		{
			if (value == null)
				return oldvalue.ToString();
			return value;
		}
		static string SafeLoad (string value, ushort oldvalue)
		{
			if (value == null)
				return oldvalue.ToString();
			return value;
		}
		static string SafeLoad (string value, float oldvalue)
		{
			if (value == null)
				return oldvalue.ToString();
			return value;
		}
		#endif


	}
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsDataConverter
{
	static class SdcSettings
	{
		public static bool EnableLogging
		{
			get
			{
				bool enabled;
				if (bool.TryParse(ConfigurationManager.AppSettings["EnableLogging"], out enabled))
				{
					return enabled;
				}
				return false;
			}
		}

		public static bool IgnoreSteam
		{
			get
			{
				bool ignore;
				if (bool.TryParse(ConfigurationManager.AppSettings["IgnoreSteam"], out ignore))
				{
					return ignore;
				}
				return false;
			}
		}

		public static FileInfo OriginalSinsExe
		{
			get
			{
				return OriginalSinsPath?.GetFiles("ConvertData.exe").FirstOrDefault();
			}
		}

		public static DirectoryInfo OriginalSinsPath
		{
			get
			{
				try
				{
					return new DirectoryInfo(ConfigurationManager.AppSettings["OriginalSinsPath"]);
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

		public static string ScriptsLocation
		{
			get
			{
				return ConfigurationManager.AppSettings["ScriptsLocation"] ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
		}

		public static void ScanForInstalls()
		{
			UpdateSetting("OriginalSinsPath", RegistryKeys.OriginalSins?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("EntrenchmentPath", RegistryKeys.Entrenchment?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("DiplomacyPath", RegistryKeys.Diplomacy?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("TrinityPath", RegistryKeys.Trinity?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("RebellionPath", RegistryKeys.Rebellion?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("SteamPath", RegistryKeys.Steam?.GetValue("Path")?.ToString() ?? "");
		}

		public static void UpdateSetting(string key, string value)
		{
			var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			var settings = configFile.AppSettings.Settings;
			if (settings[key] == null)
			{
				settings.Add(key, value);
			}
			else
			{
				settings[key].Value = value;
			}
			configFile.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
		}
	}
}

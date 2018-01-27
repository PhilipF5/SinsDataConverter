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
		public static FileInfo DiplomacyExe
		{
			get
			{
				return DiplomacyPath?.GetFiles("ConvertData_Diplomacy.exe").FirstOrDefault();
			}
		}

		public static DirectoryInfo DiplomacyPath
		{
			get
			{
				try
				{
					return new DirectoryInfo(ParseToStringOrNull("TrinityPath") ?? ParseToStringOrNull("DiplomacyPath"));
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

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

		public static FileInfo EntrenchmentExe
		{
			get
			{
				return EntrenchmentPath?.GetFiles("ConvertData_Entrenchment.exe").FirstOrDefault();
			}
		}

		public static DirectoryInfo EntrenchmentPath
		{
			get
			{
				try
				{
					return new DirectoryInfo(ParseToStringOrNull("TrinityPath") ?? ParseToStringOrNull("EntrenchmentPath"));
				}
				catch (ArgumentNullException)
				{
					return null;
				}
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
				var convertData = OriginalSinsPath?.GetFiles("ConvertData_OriginalSins.exe") ?? OriginalSinsPath?.GetFiles("ConvertData.exe");
				return convertData?.FirstOrDefault();
			}
		}

		public static DirectoryInfo OriginalSinsPath
		{
			get
			{
				try
				{
					var test = ConfigurationManager.AppSettings["TrinityPath"];
					return new DirectoryInfo(ParseToStringOrNull("TrinityPath") ?? ParseToStringOrNull("OriginalSinsPath"));
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

		public static FileInfo RebellionExe
		{
			get
			{
				return RebellionPath?.GetFiles("ConvertData_Rebellion.exe").FirstOrDefault();
			}
		}

		public static DirectoryInfo RebellionPath
		{
			get
			{
				try
				{
					if (ParseToStringOrNull("SteamPath") != null)
					{
						return new DirectoryInfo(ParseToStringOrNull("SteamPath")).GetDirectories("Sins of a Solar Empire Rebellion", SearchOption.AllDirectories).FirstOrDefault();
					}
					return new DirectoryInfo(ParseToStringOrNull("RebellionPath"));
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

		public static FileInfo GetExeOfVersion(GameVersion? version)
		{
			switch (version)
			{
				case GameVersion.OriginalSins:
					return OriginalSinsExe;
				case GameVersion.Entrenchment:
					return EntrenchmentExe;
				case GameVersion.Diplomacy:
					return DiplomacyExe;
				case GameVersion.Rebellion:
					return RebellionExe;
				default:
					throw new ArgumentOutOfRangeException("version", "Invalid version provided");
			}
		}

		public static bool HasVersion(GameVersion version)
		{
			if (GetExeOfVersion(version) != null)
			{
				return true;
			}
			return false;
		}

		public static string ParseToStringOrNull(string settingName)
		{
			var setting = ConfigurationManager.AppSettings[settingName];
			return (String.IsNullOrEmpty(setting) ? null : setting);
		}

		public static void ScanForInstalls()
		{
			UpdateSetting("OriginalSinsPath", RegistryKeys.OriginalSins?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("EntrenchmentPath", RegistryKeys.Entrenchment?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("DiplomacyPath", RegistryKeys.Diplomacy?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("TrinityPath", RegistryKeys.Trinity?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("RebellionPath", RegistryKeys.Rebellion?.GetValue("Path")?.ToString() ?? "");
			UpdateSetting("SteamPath", RegistryKeys.Steam?.GetValue("InstallPath")?.ToString() ?? "");
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

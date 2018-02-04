using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SinsDataConverter.Core
{
    public static class ExeManager
    {
		private static List<ConvertDataExe> _exeList = new List<ConvertDataExe>();

		private static bool _addFromDirectoryPath(string path, string filename, GameEdition gameEdition)
		{
			var directory = new DirectoryInfo(path);
			var convertData = directory?.GetFiles(filename);
			if (convertData.FirstOrDefault() != null)
			{
				Add(new ConvertDataExe
				{
					File = convertData.FirstOrDefault(),
					GameEdition = gameEdition,
				});
				return true;
			}
			return false;
		}

		public static void Add(ConvertDataExe exe)
		{
			if (!_exeList.Any(x => x.File.FullName == exe.File.FullName))
			{
				_exeList.Add(exe);
			}
		}

		public static FileInfo GetFile(GameEdition gameEdition, bool allowCustom = true)
		{
			return GetLatest(gameEdition, allowCustom)?.File;
		}

		public static FileInfo GetFile(GameEdition gameEdition, string gameVersion)
		{
			throw new NotImplementedException("Getting an EXE by version is not yet implemented");
		}

		public static ConvertDataExe GetLatest(GameEdition gameEdition, bool allowCustom = true)
		{
			var candidates = _exeList.Where(x => x.GameEdition == gameEdition);
			if (!allowCustom)
			{
				candidates = candidates.Where(x => !x.IsCustom);
			}
			return candidates.OrderByDescending(x => x.GameVersion).FirstOrDefault();
		}

		public static bool HasExeForEdition(GameEdition	gameEdition)
		{
			if (GetLatest(gameEdition) != null)
			{
				return true;
			}
			return false;
		}

		public static void Load(IEnumerable<ConvertDataExe> source)
		{
			_exeList = new List<ConvertDataExe>(source);
		}

		public static void Save()
		{

		}

		public static bool ScanForInstalls()
		{
			List<bool> results = new List<bool>();

			var originalSins = RegistryKeys.OriginalSins?.GetValue("Path")?.ToString();
			var entrenchment = RegistryKeys.Entrenchment?.GetValue("Path")?.ToString();
			var diplomacy = RegistryKeys.Diplomacy?.GetValue("Path")?.ToString();
			var trinity = RegistryKeys.Trinity?.GetValue("Path")?.ToString();
			var rebellion = RegistryKeys.Rebellion?.GetValue("Path")?.ToString();
			var steam = RegistryKeys.Steam?.GetValue("InstallPath")?.ToString();

			if (steam != null)
			{
				var steamInstall = new DirectoryInfo(steam);
				var steamSins = steamInstall.GetDirectories("Sins of a Solar Empire Rebellion", SearchOption.AllDirectories).FirstOrDefault();
				if (steamSins != null)
				{
					results.Add(_addFromDirectoryPath(steamSins.FullName, "ConvertData_Rebellion.exe", GameEdition.Rebellion));
				}
			}

			if (trinity != null)
			{
				results.Add(_addFromDirectoryPath(trinity, "ConvertData_OriginalSins.exe", GameEdition.OriginalSins));
				results.Add(_addFromDirectoryPath(trinity, "ConvertData_Entrenchment.exe", GameEdition.Entrenchment));
				results.Add(_addFromDirectoryPath(trinity, "ConvertData_Diplomacy.exe", GameEdition.Diplomacy));
			}

			if (originalSins != null)
			{
				results.Add(_addFromDirectoryPath(originalSins, "ConvertData.exe", GameEdition.OriginalSins));
			}

			if (entrenchment != null)
			{
				results.Add(_addFromDirectoryPath(entrenchment, "ConvertData_Entrenchment.exe", GameEdition.Entrenchment));
			}

			if (diplomacy != null)
			{
				results.Add(_addFromDirectoryPath(entrenchment, "ConvertData_Diplomacy.exe", GameEdition.Diplomacy));
			}

			if (rebellion != null)
			{
				results.Add(_addFromDirectoryPath(rebellion, "ConvertData_Rebellion.exe", GameEdition.Rebellion));
			}

			return results.Any(x => x == true);
		}
	}
}

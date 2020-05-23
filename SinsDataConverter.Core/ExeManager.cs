using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SinsDataConverter.Core
{
    public static class ExeManager
    {
		private static List<ConvertDataExe> ExeList = new List<ConvertDataExe>();

		public static void Add(ConvertDataExe exe)
		{
			if (!ExeList.Any(x => x.File.FullName == exe.File.FullName))
			{
				ExeList.Add(exe);
			}
		}

		public static FileInfo? GetFile(GameEdition gameEdition, bool allowCustom = true)
		{
			return GetLatest(gameEdition, allowCustom)?.File;
		}

		public static FileInfo GetFile(GameEdition gameEdition, string gameVersion)
		{
			throw new NotImplementedException("Getting an EXE by version is not yet implemented");
		}

		public static ConvertDataExe? GetLatest(GameEdition gameEdition, bool allowCustom = true)
		{
			return ExeList
				.Where(exe => exe.GameEdition == gameEdition)
				.Where(exe => !exe.IsCustom || allowCustom)
				.OrderByDescending(exe => exe.GameVersion)
				.FirstOrDefault();
		}

		public static bool HasExeForEdition(GameEdition	gameEdition) => GetLatest(gameEdition) != null;

		public static void Load(IEnumerable<ConvertDataExe> source)
		{
			ExeList = new List<ConvertDataExe>(source);
		}

		public static void Save()
		{
			throw new NotImplementedException("Saving exeList not yet implemented");
		}

		public static bool ScanForInstalls()
		{
			var installs = new List<GameInstall>
			{
				ClassicInstall.OriginalSins,
				ClassicInstall.Entrenchment,
				ClassicInstall.Diplomacy,
				ClassicInstall.Trinity,
				ClassicInstall.Rebellion,
				SteamInstall.Rebellion,
			};

			var exes = installs.Where(install => install.IsInstalled).SelectMany(install => install.ConvertDataExes);
			ExeList.AddRange(exes);
			return exes.Any();
		}
	}
}

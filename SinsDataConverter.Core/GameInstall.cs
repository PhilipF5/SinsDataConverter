using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SinsDataConverter.Core
{
	public class GameInstall
	{
		public IEnumerable<ConvertDataExe> ConvertDataExes
		{
			get => ConvertDataNames.Select(cd => GetConvertDataExe(cd.Key, cd.Value)).OfType<ConvertDataExe>();
		}

		public DirectoryInfo? InstallDirectory => InstallPath != null ? new DirectoryInfo(InstallPath) : null;
		public virtual string? InstallPath => RegistryKey?.GetValue("Path")?.ToString();
		public bool IsInstalled => InstallPath != null;
		public RegistryKey? RegistryKey { get; set; }

		protected IDictionary<GameEdition, string> ConvertDataNames { get; set; }

		public GameInstall(RegistryKey? registryKey, IDictionary<GameEdition, string> convertDataNames)
		{
			RegistryKey = registryKey;
			ConvertDataNames = convertDataNames;
		}

		public ConvertDataExe? GetConvertDataExe(GameEdition gameEdition, string filename)
		{
			var convertData = InstallDirectory?.GetFiles(filename)?.FirstOrDefault();
			if (convertData != null)
			{
				return new ConvertDataExe(convertData)
				{
					GameEdition = gameEdition,
				};
			}
			return null;
		}
	}
}

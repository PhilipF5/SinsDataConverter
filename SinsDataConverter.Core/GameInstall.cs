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
			get => ConvertDataNames.Select(cd => GetConvertDataExe(cd.Key, cd.Value));
		}

		public DirectoryInfo InstallDirectory => InstallPath != null ? new DirectoryInfo(InstallPath) : null;
		public string InstallPath => RegistryKey?.GetValue("Path")?.ToString();
		public bool IsInstalled => RegistryKey != null;
		public RegistryKey RegistryKey { get; set; }

		protected IDictionary<GameEdition, string> ConvertDataNames { get; set; }

		public ConvertDataExe GetConvertDataExe(GameEdition gameEdition, string filename)
		{
			var convertData = InstallDirectory?.GetFiles(filename)?.FirstOrDefault();
			if (convertData != null)
			{
				return new ConvertDataExe
				{
					File = convertData,
					GameEdition = gameEdition,
				};
			}
			return null;
		}
	}
}

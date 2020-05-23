using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SinsDataConverter.Core
{
	public class SteamInstall : GameInstall
	{
		public override string? InstallPath
		{
			get
			{
				var steamPath = RegistryKey?.GetValue("InstallPath")?.ToString();
				if (steamPath == null)
				{
					return null;
				}

				return new DirectoryInfo(steamPath)
					.GetDirectories("Sins of a Solar Empire Rebellion", SearchOption.AllDirectories)
					.Select(d => d.FullName)
					.FirstOrDefault();
			}
		}

		public static SteamInstall Rebellion
		{
			get
			{
				var convertDataNames = new Dictionary<GameEdition, string>
				{
					[GameEdition.Rebellion] = "ConvertData_Rebellion.exe",
				};
				return new SteamInstall(RegistryKeys.Steam, convertDataNames);
			}
		}

		public SteamInstall(RegistryKey? registryKey, IDictionary<GameEdition, string> convertDataNames)
			: base(registryKey, convertDataNames)
		{
		}
	}
}

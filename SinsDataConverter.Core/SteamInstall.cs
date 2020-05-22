using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SinsDataConverter.Core
{
	public class SteamInstall : GameInstall
	{
		public override string InstallPath
		{
			get
			{
				var steamPath = RegistryKey?.GetValue("InstallPath")?.ToString();
				if (steamPath == null)
				{
					return null;
				}

				return new DirectoryInfo(steamPath)
					.GetDirectories(
						"Sins of a Solar Empire Rebellion",
						SearchOption.AllDirectories
					).FirstOrDefault()?
					.FullName;
			}
		}

		public static SteamInstall Rebellion => new SteamInstall
		{
			RegistryKey = RegistryKeys.Steam,
			ConvertDataNames = new Dictionary<GameEdition, string>
			{
				{ GameEdition.Rebellion, "ConvertData_Rebellion.exe" },
			},
		};
	}
}

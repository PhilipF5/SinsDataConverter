using System.Collections.Generic;

namespace SinsDataConverter.Core
{
	public class ClassicInstall : GameInstall
	{
		public static ClassicInstall Diplomacy => new ClassicInstall
		{
			RegistryKey = RegistryKeys.Diplomacy,
			ConvertDataNames = new Dictionary<GameEdition, string>
			{
				{ GameEdition.Diplomacy, "ConvertData_Diplomacy.exe" },
			},
		};

		public static ClassicInstall Entrenchment => new ClassicInstall
		{
			RegistryKey = RegistryKeys.Entrenchment,
			ConvertDataNames = new Dictionary<GameEdition, string>
			{
				{ GameEdition.Entrenchment, "ConvertData_Entrenchment.exe" },
			},
		};

		public static ClassicInstall OriginalSins => new ClassicInstall
		{
			RegistryKey = RegistryKeys.OriginalSins,
			ConvertDataNames = new Dictionary<GameEdition, string>
			{
				{ GameEdition.OriginalSins, "ConvertData.exe" },
			},
		};

		public static ClassicInstall Rebellion => new ClassicInstall
		{
			RegistryKey = RegistryKeys.Rebellion,
			ConvertDataNames = new Dictionary<GameEdition, string>
			{
				{ GameEdition.Rebellion, "ConvertData_Rebellion.exe" },
			},
		};

		public static ClassicInstall Trinity => new ClassicInstall
		{
			RegistryKey = RegistryKeys.Trinity,
			ConvertDataNames = new Dictionary<GameEdition, string>
			{
				{ GameEdition.OriginalSins, "ConvertData_OriginalSins.exe" },
				{ GameEdition.OriginalSins, "ConvertData_Entrenchment.exe" },
				{ GameEdition.OriginalSins, "ConvertData_Diplomacy.exe" },
			},
		};
	}
}

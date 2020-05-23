using Microsoft.Win32;
using System.Collections.Generic;

namespace SinsDataConverter.Core
{
	public class ClassicInstall : GameInstall
	{
		public static ClassicInstall Diplomacy
		{
			get
			{
				var convertDataNames = new Dictionary<GameEdition, string>
				{
					[GameEdition.Diplomacy] = "ConvertData_Diplomacy.exe",
				};
				return new ClassicInstall(RegistryKeys.Diplomacy, convertDataNames);
			}
		}

		public static ClassicInstall Entrenchment
		{
			get
			{
				var convertDataNames = new Dictionary<GameEdition, string>
				{
					[GameEdition.Entrenchment] = "ConvertData_Entrenchment.exe",
				};
				return new ClassicInstall(RegistryKeys.Entrenchment, convertDataNames);
			}
		}

		public static ClassicInstall OriginalSins
		{
			get
			{
				var convertDataNames = new Dictionary<GameEdition, string>
				{
					[GameEdition.OriginalSins] = "ConvertData.exe",
				};
				return new ClassicInstall(RegistryKeys.OriginalSins, convertDataNames);
			}
		}

		public static ClassicInstall Rebellion
		{
			get
			{
				var convertDataNames = new Dictionary<GameEdition, string>
				{
					[GameEdition.Rebellion] = "ConvertData_Rebellion.exe",
				};
				return new ClassicInstall(RegistryKeys.Rebellion, convertDataNames);
			}
		}

		public static ClassicInstall Trinity
		{
			get
			{
				var convertDataNames = new Dictionary<GameEdition, string>
				{
					[GameEdition.OriginalSins] = "ConvertData_OriginalSins.exe",
					[GameEdition.Entrenchment] = "ConvertData_Entrenchment.exe",
					[GameEdition.Diplomacy] = "ConvertData_Diplomacy.exe",
				};
				return new ClassicInstall(RegistryKeys.Trinity, convertDataNames);
			}
		}

		public ClassicInstall(RegistryKey? registryKey, IDictionary<GameEdition, string> convertDataNames)
			: base(registryKey, convertDataNames)
		{
		}
	}
}

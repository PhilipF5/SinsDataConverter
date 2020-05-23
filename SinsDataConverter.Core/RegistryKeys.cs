using Microsoft.Win32;
using System;

namespace SinsDataConverter.Core
{
	static class RegistryKeys
	{
		public static bool Is64Bit => Environment.Is64BitOperatingSystem;
		public static RegistryKey? Diplomacy => Open("sinsdiplo");
		public static RegistryKey? Entrenchment => Open("sinsentrench");
		public static RegistryKey? OriginalSins => Open("sins");
		public static RegistryKey? Rebellion => Open("sinsrebellion");
		public static RegistryKey? Steam => Open($"SOFTWARE\\{(Is64Bit ? "Wow6432Node\\" : "")}Valve\\Steam", false);
		public static RegistryKey? Trinity => Open("sinstrinity");
		private static string Root => $"SOFTWARE\\{(Is64Bit ? "Wow6432Node\\" : "")}Stardock\\Drengin.net\\";

		private static RegistryKey? Open(string subKey, bool useStardockRoot = true)
		{
			return Registry.LocalMachine.OpenSubKey($"{(useStardockRoot ? Root : string.Empty)}{subKey}", false);
		}
	}
}

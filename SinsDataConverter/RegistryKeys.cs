using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SinsDataConverter
{
	static class RegistryKeys
	{
		private static string _root
		{
			get
			{
				if (Is64Bit)
				{
					return "SOFTWARE\\Wow6432Node\\Stardock\\Drengin.net\\";
				}
				else
				{
					return "SOFTWARE\\Stardock\\Drengin.net\\";
				}
			}
		}

		public static bool Is64Bit
		{
			get
			{
				return (IntPtr.Size == 8);
			}
		}

		public static RegistryKey OriginalSins
		{
			get
			{
				return Registry.LocalMachine.OpenSubKey(_root + "sins", false);
			}
		}

		public static RegistryKey Entrenchment
		{
			get
			{
				return Registry.LocalMachine.OpenSubKey(_root + "sinsentrench", false);
			}
		}

		public static RegistryKey Diplomacy
		{
			get
			{
				return Registry.LocalMachine.OpenSubKey(_root + "sinsdiplo", false);
			}
		}

		public static RegistryKey Trinity
		{
			get
			{
				return Registry.LocalMachine.OpenSubKey(_root + "sinstrinity", false);
			}
		}

		public static RegistryKey Rebellion
		{
			get
			{
				return Registry.LocalMachine.OpenSubKey(_root + "sinsrebellion", false);
			}
		}
	}
}

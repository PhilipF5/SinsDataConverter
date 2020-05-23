using System;
using System.IO;

namespace SinsDataConverter.Core
{
    public class ConvertDataExe
    {
		public FileInfo File { get; set; }
		public GameEdition? GameEdition { get; set; }
		public Version? GameVersion { get; set; }
		public bool IsCustom { get; set; }
		public bool IsSteam { get; set; }

		public ConvertDataExe(FileInfo file) => File = file;
    }
}

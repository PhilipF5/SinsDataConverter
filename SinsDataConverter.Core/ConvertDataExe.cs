using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SinsDataConverter.Core
{
    public class ConvertDataExe
    {
		public FileInfo File { get; set; }
		public GameEdition GameEdition { get; set; }
		public Version GameVersion { get; set; }
		public bool IsCustom { get; set; }
		public bool IsSteam { get; set; }
    }
}

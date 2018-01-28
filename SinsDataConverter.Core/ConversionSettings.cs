using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsDataConverter.Core
{
	public class ConversionSettings
	{
		public ConversionInputType? InputType { get; set; }
		public ConversionOutputType? OutputType { get; set; }
		public GameEdition? Version { get; set; }

		public bool IsValid()
		{
			return !(GetType().GetProperties()
				.Select(property => property.GetValue(this))
				.Any(value => value == null));
		}

		public enum ConversionInputType
		{
			File,
			Directory
		}

		public enum ConversionOutputType
		{
			Bin,
			Txt
		}
	}
}

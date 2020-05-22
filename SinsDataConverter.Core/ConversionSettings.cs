using System.Linq;

namespace SinsDataConverter.Core
{
	public class ConversionSettings
	{
		public ConversionInputType? InputType { get; set; }
		public ConversionOutputType? OutputType { get; set; }
		public GameEdition? Version { get; set; }

		public bool IsValid
		{
			get
			{
				return GetType().GetProperties()
					.Where(property => property.Name != "IsValid")
					.All(property => property.GetValue(this) != null);
			}
		}

		public enum ConversionInputType
		{
			File,
			Directory,
		}

		public enum ConversionOutputType
		{
			Bin,
			Txt,
		}
	}
}

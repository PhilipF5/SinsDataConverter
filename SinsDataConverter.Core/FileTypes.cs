using System.Linq;

namespace SinsDataConverter.Core
{
	public class FileType
	{
		public string Extension => "." + Name;
		public string Name { get; set; }
		public string Pattern => $"*{Extension}";

		public static FileType Brushes { get; } = new FileType("brushes");
		public static FileType Entity { get; } = new FileType("entity");
		public static FileType Mesh { get; } = new FileType("mesh");
		public static FileType Particle { get; } = new FileType("particle");
		public static FileType[] All { get; } = { Brushes, Entity, Mesh, Particle };

		public FileType(string name)
		{
			Name = name;
		}

		public static FileType GetFromExtension(string extension) => All.FirstOrDefault(x => x.Extension == extension);
	}
}

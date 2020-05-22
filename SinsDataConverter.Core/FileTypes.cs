using System.Linq;

namespace SinsDataConverter.Core
{
	public class FileType
	{
		public string Extension => "." + Name;
		public string Name { get; set; }
		public string Pattern => $"*{Extension}";

		public static FileType Brushes { get; } = new FileType
		{
			Name = "brushes"
		};

		public static FileType Entity { get; } = new FileType
		{
			Name = "entity"
		};

		public static FileType Mesh { get; } = new FileType
		{
			Name = "mesh"
		};

		public static FileType Particle { get; } = new FileType
		{
			Name = "particle"
		};

		public static FileType[] All { get; } = { Brushes, Entity, Mesh, Particle };

		public static FileType GetFromExtension(string extension)
		{
			return All.FirstOrDefault(x => x.Extension == extension);
		}
	}
}

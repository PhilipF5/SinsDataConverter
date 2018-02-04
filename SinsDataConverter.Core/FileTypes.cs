using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsDataConverter.Core
{
	public static class FileTypes
	{
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
			return All.Where(x => x.Extension == extension).FirstOrDefault();
		}
	}

	public class FileType
	{
		public string Extension
		{
			get
			{
				return "." + Name;
			}
		}
		public string Name { get; set; }
	}
}

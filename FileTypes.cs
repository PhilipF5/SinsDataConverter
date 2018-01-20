using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsDataConverter
{
	static class FileTypes
	{
		public static FileType Brushes = new FileType()
		{
			Name = "brushes"
		};

		public static FileType Entity = new FileType()
		{
			Name = "entity"
		};

		public static FileType Mesh = new FileType()
		{
			Name = "mesh"
		};

		public static FileType Particle = new FileType()
		{
			Name = "particle"
		};

		public static FileType[] All = { Brushes, Entity, Mesh, Particle };

		public static FileType GetFromExtension(string extension)
		{
			return All.Where(x => x.Extension == extension).FirstOrDefault();
		}
	}

	class FileType
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

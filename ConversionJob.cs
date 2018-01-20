using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SinsDataConverter
{
	class ConversionJob
	{
		private bool _directionToTxt;
		private FileInfo _exe;
		private FileInfo _outputFile;
		private bool _overwrite;
		private FileInfo _sourceFile;
		private FileType _type;

		public bool ConvertToBin
		{
			get
			{
				return !_directionToTxt;
			}
		}

		public bool ConvertToTxt
		{
			get
			{
				return _directionToTxt;
			}
		}

		public string ExePath
		{
			get
			{
				return _exe.FullName;
			}
			set
			{
				var file = new FileInfo(value);
				if (!file.Exists)
				{
					throw new FileNotFoundException(file.Name + " could not be found", file.FullName);
				}
				_exe = file;
			}
		}

		public string FileName
		{
			get
			{
				return _sourceFile.Name;
			}
		}

		public bool IsInPlace
		{
			get
			{
				return _sourceFile.FullName == _outputFile.FullName;
			}
			set
			{
				if (value)
				{
					_outputFile = new FileInfo(_sourceFile.FullName);
				}
			}
		}

		public string OutputPath
		{
			get
			{
				return _outputFile.FullName;
			}
			set
			{
				var file = new FileInfo(value);
				if (file.Exists)
				{
					_overwrite = true;
				}
			}
		}

		public string SourcePath
		{
			get
			{
				return _sourceFile.FullName;
			}
			set
			{
				var file = new FileInfo(value);
				if (!file.Exists)
				{
					throw new FileNotFoundException(file.Name + " could not be found", file.FullName);
				}
				_sourceFile = file;
			}
		}

		public bool WillOverwrite
		{
			get
			{
				return _overwrite;
			}
		}

		public override string ToString()
		{
			return $"\"{ExePath}\" {_type.Name} \"{SourcePath}\" \"{OutputPath}\"{(ConvertToTxt ? " txt" : "")}";
		}
	}
}

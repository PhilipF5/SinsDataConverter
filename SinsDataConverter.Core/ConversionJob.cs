using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SinsDataConverter.Core
{
	public class ConversionJob
	{
		private bool DirectionToTxt;
		private FileInfo Exe;
		private FileInfo OutputFile;
		private bool Overwrite;
		private FileInfo SourceFile;

		public bool ConvertToBin
		{
			get => !DirectionToTxt;
			set => DirectionToTxt = !value;
		}

		public bool ConvertToTxt
		{
			get => DirectionToTxt;
			set => DirectionToTxt = value;
		}

		public string ExePath
		{
			get => Exe.FullName;
			set => Exe = GetFile(value);
		}

		public string FileName => SourceFile.Name;

		public bool IsInPlace
		{
			get => SourceFile.FullName == OutputFile.FullName;
			set
			{
				if (value)
				{
					OutputPath = SourceFile.FullName;
				}
			}
		}

		public string OutputPath
		{
			get => OutputFile.FullName;
			set
			{
				var file = new FileInfo(value);
				if (file.Exists)
				{
					Overwrite = true;
				}
				OutputFile = file;
			}
		}

		public string SourcePath
		{
			get => SourceFile.FullName;
			set => SourceFile = GetFile(value);
		}

		public FileType Type => FileType.GetFromExtension(SourceFile.Extension);
		public bool WillOverwrite => Overwrite;

		private ConversionJob(bool directionToTxt, string exePath, string outputPath, string sourcePath)
		{
			DirectionToTxt = directionToTxt;
			Exe = GetFile(exePath);
			SourceFile = GetFile(sourcePath);

			var outputFile = new FileInfo(outputPath);
			OutputFile = outputFile;
			if (outputFile.Exists)
			{
				Overwrite = true;
			}
		}

		public static IEnumerable<ConversionJob> Create(string inputPath, string outputPath, ConversionSettings settings)
		{
			var exe = ExeManager.GetFile(settings.Version ??
				throw new ArgumentNullException("gameEdition", "No valid game edition provided for this input path"));
			var convertToTxt = (settings.OutputType == ConversionSettings.ConversionOutputType.Txt);
			var output = new DirectoryInfo(outputPath);
			if (!output.Exists)
			{
				throw new DirectoryNotFoundException("Output directory does not exist");
			}

			switch (settings.InputType) {
				case ConversionSettings.ConversionInputType.File:
					var file = new FileInfo(inputPath);
					if (!file.Exists)
					{
						throw new FileNotFoundException("Input file does not exist", inputPath);
					}

					return new List<ConversionJob>
					{
						Create(file, output, exe!, convertToTxt),
					};
				case ConversionSettings.ConversionInputType.Directory:
					var directory = new DirectoryInfo(inputPath);
					if (!directory.Exists)
					{
						throw new DirectoryNotFoundException("Input directory does not exist");
					}

					if (exe == null || !exe.Exists)
					{
						throw new FileNotFoundException(
							$"ConvertData EXE does not exist for version {Enum.GetName(typeof(GameEdition), settings.Version)}");
					}

					return Create(directory, output, exe, convertToTxt);
				default:
					throw new ArgumentNullException("settings", "Input type not specified");
			}
		}

		public static ConversionJob Create(FileInfo file, DirectoryInfo output, FileInfo exe, bool convertToTxt = false)
		{
			return new ConversionJob(
				directionToTxt: convertToTxt,
				exePath: exe.FullName,
				outputPath: file.FullName.Replace(file.DirectoryName, output.FullName),
				sourcePath: file.FullName
			);
		}

		public static IEnumerable<ConversionJob> Create(
			DirectoryInfo folder,
			DirectoryInfo output,
			FileInfo exe,
			bool convertToTxt = false
		) {
			return FileType.All
				.SelectMany(type => folder.EnumerateFiles($"*{type.Extension}", SearchOption.AllDirectories))
				.Select(file => new ConversionJob(
					directionToTxt: convertToTxt,
					exePath: exe.FullName,
					outputPath: file.FullName.Replace(folder.FullName, output.FullName),
					sourcePath: file.FullName
				));
		}

		public override string ToString()
		{
			return $"\"{ExePath}\" {Type.Name} \"{SourcePath}\" \"{OutputPath}\"{(ConvertToTxt ? " txt" : "")}";
		}

		private FileInfo GetFile(string path)
		{
			var file = new FileInfo(path);
			if (!file.Exists)
			{
				throw new FileNotFoundException($"{file.Name} could not be found", file.FullName);
			}
			return file;
		}
	}
}

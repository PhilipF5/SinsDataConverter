using RunProcessAsTask;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SinsDataConverter.Core
{
	public static class ConversionEngine
	{
		private static List<ConversionJob> Jobs;
		private static bool KeepScripts;
		private static FileInfo ScriptFile;
		private static DirectoryInfo ScriptsLocation;
		public static ReadOnlyCollection<ConversionJob> Queue => Jobs.AsReadOnly();

		public static void AddJob(ConversionJob job)
		{
			Jobs.Add(job);
		}

		public static void AddJobs(IEnumerable<ConversionJob> jobs)
		{
			Jobs.AddRange(jobs);
		}

		public static void CreateScriptFile()
		{
			var builder = new ScriptBuilder();
			builder.AddJobs(Jobs);
			ScriptFile = new FileInfo($"{ScriptsLocation.FullName}\\{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}.bat");
			using (var fileStream = ScriptFile.OpenWrite())
			{
				builder.Build(fileStream);
			}
		}

		public static void RemoveJob(ConversionJob job)
		{
			Jobs.Remove(job);
		}

		public static async Task Run()
		{
			CreateScriptFile();

			var batchProcess = new ProcessStartInfo
			{
				CreateNoWindow = true,
				FileName = ScriptFile.FullName
			};

			await ProcessEx.RunAsync(batchProcess);

			if (!KeepScripts)
			{
				ScriptFile.Delete();
			}
		}

		public static void StartNew(string scriptsLocation = null, bool keepScripts = false)
		{
			Jobs = new List<ConversionJob>();
			KeepScripts = keepScripts;
			ScriptsLocation =
				new DirectoryInfo(scriptsLocation ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
		}
	}
}

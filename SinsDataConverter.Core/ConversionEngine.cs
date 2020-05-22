using RunProcessAsTask;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SinsDataConverter.Core
{
	public class ConversionEngine
	{
		private List<ConversionJob> Jobs;
		private bool KeepScripts;
		private FileInfo ScriptFile;
		private DirectoryInfo ScriptsLocation;
		public ReadOnlyCollection<ConversionJob> Queue => Jobs.AsReadOnly();

		public ConversionEngine(string scriptsLocation = null, bool keepScripts = false)
		{
			Jobs = new List<ConversionJob>();
			KeepScripts = keepScripts;
			ScriptsLocation =
				new DirectoryInfo(scriptsLocation ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
		}

		public void AddJob(ConversionJob job)
		{
			Jobs.Add(job);
		}

		public void AddJobs(IEnumerable<ConversionJob> jobs)
		{
			Jobs.AddRange(jobs);
		}

		public void CreateScriptFile()
		{
			var builder = new ScriptBuilder();
			builder.AddJobs(Jobs);
			ScriptFile = new FileInfo($"{ScriptsLocation.FullName}\\{DateTime.Now.ToString("yyyy-MM-dd_HHmmss")}.bat");
			using (var fileStream = ScriptFile.OpenWrite())
			{
				builder.Build(fileStream);
			}
		}

		public void RemoveJob(ConversionJob job)
		{
			Jobs.Remove(job);
		}

		public async Task Run()
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
	}
}

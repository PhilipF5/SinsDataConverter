using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using RunProcessAsTask;

namespace SinsDataConverter
{
	static class ConversionEngine
	{
		private static List<ConversionJob> _jobs;
		private static FileInfo _scriptFile;
		private static DirectoryInfo _scriptsLocation = new DirectoryInfo(SdcSettings.ScriptsLocation);

		public static ReadOnlyCollection<ConversionJob> Queue
		{
			get
			{
				return _jobs.AsReadOnly();
			}
		}

		private static void _createScriptFile()
		{
			var builder = new ScriptBuilder();
			builder.AddJobs(_jobs);
			_scriptFile = new FileInfo(_scriptsLocation.FullName + "\\" + DateTime.Now.ToString("YYYY-MM-DD_HHmmss") + ".bat");
			using (var fileStream = _scriptFile.OpenWrite())
			{
				builder.Build(fileStream);
			}
		}

		public static void AddJob(ConversionJob job)
		{
			_jobs.Add(job);
		}

		public static void RemoveJob(ConversionJob job)
		{
			_jobs.Remove(job);
		}

		public static async Task Run()
		{
			var batchProcess = new ProcessStartInfo()
			{
				FileName = _scriptFile.FullName,
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Minimized
			};

			var batchResults = await ProcessEx.RunAsync(batchProcess);

			if (!SdcSettings.EnableLogging)
			{
				_scriptFile.Delete();
			}
		}

		public static void StartNew()
		{
			_jobs = new List<ConversionJob>();
		}
	}
}

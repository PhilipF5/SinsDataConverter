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

namespace SinsDataConverter.Core
{
	public static class ConversionEngine
	{
		private static List<ConversionJob> _jobs;
		private static bool _keepScripts;
		private static FileInfo _scriptFile;
		private static DirectoryInfo _scriptsLocation;

		public static ReadOnlyCollection<ConversionJob> Queue
		{
			get
			{
				return _jobs.AsReadOnly();
			}
		}

		public static void AddJob(ConversionJob job)
		{
			_jobs.Add(job);
		}

		public static void AddJobs(IEnumerable<ConversionJob> jobs)
		{
			_jobs.AddRange(jobs);
		}

		public static void CreateScriptFile()
		{
			var builder = new ScriptBuilder();
			builder.AddJobs(_jobs);
			_scriptFile = new FileInfo(_scriptsLocation.FullName + "\\" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".bat");
			using (var fileStream = _scriptFile.OpenWrite())
			{
				builder.Build(fileStream);
			}
		}

		public static void RemoveJob(ConversionJob job)
		{
			_jobs.Remove(job);
		}

		public static async Task Run()
		{
			CreateScriptFile();

			var batchProcess = new ProcessStartInfo()
			{
				CreateNoWindow = true,
				FileName = _scriptFile.FullName
			};

			var batchResults = await ProcessEx.RunAsync(batchProcess);

			if (!_keepScripts)
			{
				_scriptFile.Delete();
			}
		}

		public static void StartNew(string scriptsLocation, bool keepScripts = false)
		{
			_jobs = new List<ConversionJob>();
			_keepScripts = keepScripts;
			_scriptsLocation = new DirectoryInfo(scriptsLocation);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace SinsDataConverter
{
	static class ConversionEngine
	{
		private static ScriptBuilder _builder;
		private static FileStream _fileStream;
		private static List<ConversionJob> _jobs;
		private static FileInfo _scriptFile;
		private static DirectoryInfo _scriptsLocation = new DirectoryInfo(SdcSettings.ScriptsLocation);

		public static bool EnableLogging
		{
			get
			{
				bool enabled;
				if (bool.TryParse(ConfigurationManager.AppSettings["EnableLogging"], out enabled))
				{
					return enabled;
				}
				return false;
			}
		}

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

		public static void OpenScriptFile()
		{
			_scriptFile = new FileInfo(_scriptsLocation.FullName + "\\" + DateTime.Now.ToString("YYYY-MM-DD_HHmmss") + ".bat");
			_fileStream = _scriptFile.OpenWrite();
		}

		public static void RemoveJob(ConversionJob job)
		{
			_jobs.Remove(job);
		}

		public static async Task Run()
		{
			_builder.AddJobs(_jobs);
			_builder.Build(_fileStream);
			_fileStream.Close();

			Process.Start(_scriptFile.FullName);

			if (!EnableLogging)
			{
				_scriptFile.Delete();
			}
		}

		public static void StartNew()
		{
			_builder = new ScriptBuilder();
			_jobs = new List<ConversionJob>();
		}
	}
}

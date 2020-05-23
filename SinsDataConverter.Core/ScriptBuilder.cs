using System;
using System.Collections.Generic;
using System.IO;

namespace SinsDataConverter.Core
{
	class ScriptBuilder
	{
		private readonly List<ConversionJob> Jobs = new List<ConversionJob>();
		private readonly DateTime Timestamp = DateTime.UtcNow;

		public void AddJobs(IEnumerable<ConversionJob> jobs)
		{
			Jobs.AddRange(jobs);
		}

		public Stream Build(Stream? outputStream = null)
		{
			var stream = outputStream ?? new MemoryStream();
			using (var writer = new StreamWriter(stream))
			{
				foreach (var job in Jobs)
				{
					writer.WriteLine(job.ToString());
				}
			}
			return stream;
		}
	}
}

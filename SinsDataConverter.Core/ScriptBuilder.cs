using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SinsDataConverter.Core
{
	class ScriptBuilder
	{
		private readonly List<ConversionJob> _jobs = new List<ConversionJob>();
		private readonly DateTime _timestamp = DateTime.UtcNow;

		public void AddJobs(IEnumerable<ConversionJob> jobs)
		{
			_jobs.AddRange(jobs);
		}

		public Stream Build(Stream outputStream = null)
		{
			var stream = outputStream ?? new MemoryStream();
			using (var writer = new StreamWriter(stream))
			{
				foreach (var job in _jobs)
				{
					writer.WriteLine(job.ToString());
				}
			}
			return stream;
		}
	}
}

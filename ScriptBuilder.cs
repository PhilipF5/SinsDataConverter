using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SinsDataConverter
{
	class ScriptBuilder
	{
		private List<ConversionJob> _jobs = new List<ConversionJob>();
		private DateTime _timestamp = DateTime.UtcNow;

		public void AddJobs(IEnumerable<ConversionJob> jobs)
		{
			_jobs.AddRange(jobs);
		}

		public Stream Build(Stream outputStream = null)
		{
			if (outputStream == null)
			{
				outputStream = new MemoryStream();
			}
			using (var writer = new StreamWriter(outputStream))
			{
				foreach (var job in _jobs)
				{
					writer.WriteLine(job.ToString());
				}
			}
			return outputStream;
		}
	}
}

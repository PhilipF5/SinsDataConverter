using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinsDataConverter
{
	class ScriptBuilder
	{
		private List<ConversionJob> _jobs;
		private DateTime _timestamp = DateTime.UtcNow;

		public void LoadJobs(IEnumerable<ConversionJob> jobs)
		{

		}
	}
}

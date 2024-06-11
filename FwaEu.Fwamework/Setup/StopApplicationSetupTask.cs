using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class StopApplicationSetupTask : ISetupTask
	{
		private readonly IHostApplicationLifetime _applicationLifetime;

		public StopApplicationSetupTask(IHostApplicationLifetime applicationLifetime)
		{
			_applicationLifetime = applicationLifetime;
		}

		public string Name => "StopApplication";

		public Type ArgumentsType => null;

		public Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			_applicationLifetime.StopApplication();
			return Task.FromResult((ISetupTaskResult)new NoDataSetupTaskResult(new ProcessResults.ProcessResult()));
		}
	}
}

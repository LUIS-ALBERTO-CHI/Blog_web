using FwaEu.Fwamework.Temporal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public interface IScheduledBackgroundTaskFactory
	{
		/// <summary>
		/// Used to (sometimes) create a task which will cleanup, free resources, etc.
		/// </summary>
		/// <returns>Returns null if nothing to do.</returns>
		Task<ITaskStartParameters> GetScheduledTaskStartParametersAsync(IServiceProvider serviceProvider);
	}

	public class ScheduledBackgroundTaskFactory<TTask> : IScheduledBackgroundTaskFactory
		where TTask : IBackgroundTask
	{
		private readonly string _taskName;
		private readonly TimeSpan _regularity;
		private readonly ILogger<ScheduledBackgroundTaskFactory<TTask>> _logger;

		private DateTime? _lastTaskCreation;

		public ScheduledBackgroundTaskFactory(string taskName, TimeSpan regularity,
			ILogger<ScheduledBackgroundTaskFactory<TTask>> logger)
		{
			this._regularity = regularity;

			this._taskName = taskName
				?? throw new ArgumentNullException(nameof(taskName));

			this._logger = logger
				?? throw new ArgumentNullException(nameof(logger));
		}

		public Task<ITaskStartParameters> GetScheduledTaskStartParametersAsync(IServiceProvider serviceProvider)
		{
			var now = serviceProvider.GetRequiredService<ICurrentDateTime>().Now;

			if (this._lastTaskCreation == null
				|| (now - this._lastTaskCreation.Value) > this._regularity)
			{
				this._logger.LogInformation($"Creating scheduled task '{this._taskName}'. Previous creation was on '{this._lastTaskCreation}'.");
				this._lastTaskCreation = now;
				return Task.FromResult<ITaskStartParameters>(new TaskStartParameters(this._taskName, null));
			}

			return Task.FromResult<ITaskStartParameters>(null);
		}
	}
}

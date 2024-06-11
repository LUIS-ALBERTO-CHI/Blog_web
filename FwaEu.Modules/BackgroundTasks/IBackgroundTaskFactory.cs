using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public interface IBackgroundTaskFactory
	{
		IBackgroundTask CreateTask(string taskName, IServiceProvider serviceProvider);
	}

	public class BackgroundTaskFactory<TTask> : IBackgroundTaskFactory
		where TTask : class, IBackgroundTask
	{
		public BackgroundTaskFactory(string taskName)
		{
			this.TaskName = taskName ?? throw new ArgumentNullException(nameof(taskName));
		}

		public string TaskName { get; }

		public IBackgroundTask CreateTask(string taskName, IServiceProvider serviceProvider)
		{
			if (taskName == this.TaskName)
			{
				return serviceProvider.GetRequiredService<TTask>();
			}

			return null;
		}
	}

	public interface IBackgroundTask
	{
		Task<string> ExecuteAsync(ITaskStartParameters taskStartParameters, CancellationToken cancellationToken);
	}
}

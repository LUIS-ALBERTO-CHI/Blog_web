using FwaEu.Fwamework.Temporal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.BackgroundTasks
{
	public static class BackgroundTaskServiceCollectionExtensions
	{
		public static void AddBackgroundTask<TTask>(this IServiceCollection services, string taskName)
			where TTask : class, IBackgroundTask
		{
			services.AddTransient<IBackgroundTaskFactory>(sp => new BackgroundTaskFactory<TTask>(taskName));
			services.AddTransient<TTask>();
		}

		public static void AddScheduledBackgroundTask<TTask>(this IServiceCollection services,
			string taskName, TimeSpan regularity)
			where TTask : class, IBackgroundTask
		{
			services.AddSingleton<IScheduledBackgroundTaskFactory>(
				sp => new ScheduledBackgroundTaskFactory<TTask>(taskName, regularity,
					sp.GetService<ILogger<ScheduledBackgroundTaskFactory<TTask>>>()));

			services.AddBackgroundTask<TTask>(taskName);
		}
	}
}

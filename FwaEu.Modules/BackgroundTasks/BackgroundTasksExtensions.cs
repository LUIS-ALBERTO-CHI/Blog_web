using FwaEu.Fwamework.Setup;
using FwaEu.Modules.BackgroundTasks.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.BackgroundTasks
{
	public static class BackgroundTasksExtensions
	{
		public static IServiceCollection AddFwameworkModuleBackgroundTasksServices(
			this IServiceCollection services)
		{
			services.AddHostedService<BackgroundTasksBackgroundService>();
			services.AddScoped<IBackgroundTasksService, InMemoryBackgroundTasksService>();

			services.AddTransient<ISetupTask, BackgroundTasksSetupTask>();
			return services;
		}
	}
}

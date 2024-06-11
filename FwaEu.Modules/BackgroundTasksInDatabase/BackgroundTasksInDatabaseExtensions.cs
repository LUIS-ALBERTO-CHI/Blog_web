using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.BackgroundTasksInDatabase
{
	public static class BackgroundTasksInDatabaseExtensions
	{
		public static IServiceCollection AddFwameworkModuleBackgroundTasksInDatabaseServices(
			this IServiceCollection services, ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<BackgroundTaskEntityRepository>();

			//NOTE: Remove default descriptor (FwaEu.Modules.BackgroundTasks.InMemoryTaskExecution)
			services.Remove(services.First(sd => sd.ServiceType == typeof(IBackgroundTasksService)));
			services.AddScoped<IBackgroundTasksService, InDatabaseBackgroundTasksService>();

			return services;
		}
	}
}

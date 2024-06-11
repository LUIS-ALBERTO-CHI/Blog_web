using FwaEu.Fwamework;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.BackgroundTasks;
using FwaEu.Modules.Reports.BackgroundTasks;
using FwaEu.Modules.Reports.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportServiceBuilder
	{
		public ReportServiceBuilder(IServiceCollection serviceCollection)
		{
			this.ServiceCollection = serviceCollection
				?? throw new ArgumentNullException(nameof(serviceCollection));
		}

		public IServiceCollection ServiceCollection { get; }
		private bool _memoryCacheOrCleanUpBackgroundTaskAdded = false;

		private void CheckState()
		{
			if (this._memoryCacheOrCleanUpBackgroundTaskAdded)
			{
				throw new NotSupportedException("Do not use MemoryCache and " +
					"Scheduled Background CleanUp together: there is nothing to clean.");
			}
		}

		public ReportServiceBuilder AddMemoryCacheAsyncReportDataStoreService()
		{
			this.CheckState();
			this._memoryCacheOrCleanUpBackgroundTaskAdded = true;

			this.ServiceCollection.AddTransient<IAsyncReportDataStoreService, MemoryCacheAsyncReportDataStoreService>();

			return this;
		}

		public ReportServiceBuilder AddCleanUpScheduledBackgroundTask(TimeSpan regularity)
		{
			this.CheckState();
			this._memoryCacheOrCleanUpBackgroundTaskAdded = true;

			this.ServiceCollection.AddScheduledBackgroundTask<CleanUpDataStoreBackgroundTask>(
				CleanUpDataStoreBackgroundTask.TaskName, regularity);

			return this;
		}
	}

	public static class ReportsExtensions
	{
		public static IServiceCollection AddFwameworkModuleReports(
			this IServiceCollection services, Action<ReportServiceBuilder> build)
		{
			services.AddTransient<IReportService, DefaultReportService>();
			services.AddTransient<IDataService, DefaultDataService>();
			services.AddTransient<IReportDataService, DefaultReportDataService>();
			services.AddTransient<IReportDataLoadingService, DefaultReportDataLoadingService>();
			services.AddTransient<IReportFiltersDataService, DefaultReportFiltersDataService>();
			services.AddTransient<IReportFieldsDataService, DefaultReportFieldsDataService>();

			services.AddScoped<IParametersService, DefaultParametersService>();

			services.AddTransient<IParametersProvider, CultureParametersProvider>();
			services.AddTransient<IParametersProvider, CurrentUserParametersProvider>();
			services.AddTransient<IParametersProvider, DateParametersProvider>();

			services.AddScoped<FiltersParametersProvider>(); //NOTE: This one is Scoped, not Transient, because the Controller provide the parameters from body of the request
			services.AddScoped<IParametersProvider>(sp => sp.GetService<FiltersParametersProvider>()); //NOTE: This one is Scoped, not Transient, because the Controller provide the parameters from body of the request

			services.AddTransient<IReportDataProviderFactory, SqlReportDataProviderFactory>();
			services.AddTransient<SqlReportDataProvider>();
			services.AddTransient<IReportDataProvider>(sp => sp.GetRequiredService<SqlReportDataProvider>());

			services.AddTransient<IReportDataProviderFactory, ClientSideCustomReportDataProviderFactory>();
			services.AddTransient<ClientSideCustomReportDataProvider>();
			services.AddTransient<IReportDataProvider>(sp => sp.GetRequiredService<ClientSideCustomReportDataProvider>());

			services.AddTransient<IAsyncReportService, DefaultAsyncReportService>();
			services.AddTransient<IAsyncReportResultService, DefaultAsyncReportResultService>();
			services.AddBackgroundTask<ReportLoadDataBackgroundTask>(ReportLoadDataBackgroundTask.TaskName);

			build(new ReportServiceBuilder(services));

			services.AddTransient<IReportParametersValidationService, ReportParametersValidationService>();
			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<ReportPermissionProvider>>();

			return services;
		}
	}
}

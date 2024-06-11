using FwaEu.Fwamework;
using FwaEu.Fwamework.Application;
using FwaEu.Modules.Logging.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;


namespace FwaEu.Modules.Logging
{
	public static class AddFwameworkSerilogExtensions
	{
		public static IServiceCollection AddFwameworkModuleSerilog(this IServiceCollection services, ApplicationInitializationContext context)
		{
			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.Services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>((IServiceProvider services) =>
				{
					var configuration = services.GetRequiredService<IConfigurationRoot>();
					var environment = services.GetRequiredService<IWebHostEnvironment>();
					var applicationInfo = services.GetRequiredService<IApplicationInfo>();
					var logger = CreateDefaultLogger(configuration, environment, applicationInfo);
					return new SerilogLoggerProvider(logger, dispose: true);
				});
				loggingBuilder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
			});
			//NOTE : When true, dispose logger when the framework dispose the provider.
			// If the logger is not specified but dispose is true, the Log.CloseAndFlush() method 
			// will be called on the static Log class instead.

			services.AddTransient<ILogEnricherProvider, RouteLogEnricherProvider>();

			return services;
		}

		public static IApplicationBuilder ConfigureFwameworkSerilog(this IApplicationBuilder builder, IConfigurationRoot configuration,
			IWebHostEnvironment environment, IApplicationInfo applicationInfo)
		{
			Log.Logger = CreateDefaultLogger(configuration, environment, applicationInfo);
			return builder;
		}

		private static Serilog.ILogger CreateDefaultLogger(IConfigurationRoot configuration, IWebHostEnvironment environment, IApplicationInfo applicationInfo)
		{
			return new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.WithProperty("Environment", environment.EnvironmentName)
				.Enrich.WithProperty("Host", Environment.MachineName)
				.Enrich.WithProperty("ApplicationName", applicationInfo.Name)
				.Enrich.WithProperty("Version", applicationInfo.Version)
				.Enrich.FromLogContext()
				.CreateLogger();
		}
	}
}

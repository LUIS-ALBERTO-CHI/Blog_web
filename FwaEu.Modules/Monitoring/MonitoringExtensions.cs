using FwaEu.Fwamework;
using FwaEu.Fwamework.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Monitoring
{
	public static class MonitoringExtensions
	{
		public static IServiceCollection AddFwameworkModuleMonitoring(this IServiceCollection services)
		{
			services.AddTransient<IMonitoringService, DefaultMonitoringService>();
			return services;
		}

		public static IServiceCollection AddFwameworkModuleAllowApplicationEnabler(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var serviceConfiguration = context.Configuration.GetSection("Fwamework:PingAllowApplication");
			services.Configure<PingAllowApplicationOptions>(serviceConfiguration);

			return services;
		}
	}
}

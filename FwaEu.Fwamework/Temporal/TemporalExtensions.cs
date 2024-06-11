using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Temporal
{
	public static class TemporalExtensions
	{
		public static IServiceCollection AddFwameworkTemporal(this IServiceCollection services)
		{
			services.AddSingleton<ICurrentDateTime, DateTimeNowCurrentDateTime>();
			services.AddSingleton<IApplicationStartDate>(new DefaultApplicationStartDate());
			return services;
		}
	}
}

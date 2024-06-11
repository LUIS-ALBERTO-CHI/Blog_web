using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public static class GlobalizationExtensions
	{
		public static IServiceCollection AddFwameworkGlobalization(this IServiceCollection services)
		{
			services.AddTransient<ICultureResolver, DefaultCultureResolver>();
			services.AddScoped<IUserContextLanguage, HttpContextUserContextLanguage>();
			return services;
		}
	}
}

using FwaEu.Modules.Reports;
using FwaEu.BlogWeb.ViewContext.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.ViewContext
{
	public static class ViewContextExtensions
	{
		public static IServiceCollection AddApplicationViewContext(this IServiceCollection services)
		{
			services.AddScoped<IViewContextService, HttpHeaderViewContextService>();
			services.AddTransient<IParametersProvider, ViewContextParametersProvider>();

			return services;
		}
	}
}

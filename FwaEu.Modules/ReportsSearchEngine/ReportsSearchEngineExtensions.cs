using FwaEu.Modules.SearchEngine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsSearchEngine
{
	public static class ReportsSearchEngineExtensions
	{
		public static IServiceCollection AddFwameworkModuleReportsSearchEngine(this IServiceCollection services)
		{
			services.AddSearchEngineResultProvider<ReportSearchEngineResultProvider>("Reports");
			return services;
		}
	}
}

using FwaEu.Fwamework.Imports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public static class ExcelImporterExtensions
	{
		public static IServiceCollection AddFwameworkModuleExcelImportServices(this IServiceCollection services)
		{
			services.AddTransient<IImportSessionFactory, ExcelImportSessionFactory>();
			return services;
		}
	}
}

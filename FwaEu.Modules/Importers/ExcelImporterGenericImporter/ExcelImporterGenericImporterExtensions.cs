using FwaEu.Modules.Importers.ExcelImporter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.ExcelImporterGenericImporter
{
	public static class ExcelImporterGenericImporterExtensions
	{
		public static IServiceCollection AddFwameworkModuleExcelGenericImporter(this IServiceCollection services)
		{
			services.AddTransient<IExcelImportFileSessionFactory, ModelImporterExcelImportFileSessionFactory>();
			return services;
		}
	}
}

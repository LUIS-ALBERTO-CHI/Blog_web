using FwaEu.Fwamework.Imports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.SqlImporter
{
	public static class SqlImporterExtensions
	{
		public static IServiceCollection AddFwameworkModuleSqlImportServices(this IServiceCollection services)
		{
			services.AddTransient<IImportSessionFactory, SqlImportSessionFactory>();
			return services;
		}
	}
}

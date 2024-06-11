using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.DataImport
{
    public static class DataImportExtensions
    {
        public static IServiceCollection AddFwameworkModuleDataImport(this IServiceCollection services)
        {
            services.AddTransient<IDataImportService, DefaultDataImportService>();
            services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<DataImportPermissionProvider>>();
            return services;
        }
    }
}

using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public static class PermissionsExtensions
	{
		public static IServiceCollection AddFwameworkPermissions(this IServiceCollection services, ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<PermissionEntityRepository>();

			services.AddSingleton<IPermissionProviderRegister, PermissionProviderRegister>();
			services.AddTransient(typeof(IPermissionProviderAccessor<>), typeof(PermissionProviderAccessor<>));

			services.AddTransient<SimplePermissionManager>();
			services.AddTransient<CurrentUserPermissionService>();

			return services;
		}
	}
}

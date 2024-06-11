using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Permissions.ByUser.Part;
using FwaEu.Modules.Permissions.ByUser.Part.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByUser
{
	public static class ByUserExtensions
	{
		public static IServiceCollection AddFwameworkModulePermissionsByUser(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserPermissionEntityRepository>();

			services.AddTransient<IPermissionManager, ByUserPermissionManager>();
			
			services.AddTransient<IPartHandler, PermissionsPartHandler>();

			return services;
		}
	}
}

using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users.Parts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public static class UsersExtensions
	{
		public static IServiceCollection AddFwameworkUsersServices(this IServiceCollection services)
		{
			services.AddTransient<IUsersByIdentityCache, DefaultUsersByIdentityCache>();
			services.AddScoped<ICurrentUserService, HttpContextIdentityCurrentUserService>();

			services.AddTransient<IPartServiceFactory, DefaultPartServiceFactory>();
			services.AddTransient<IPartService, DefaultPartService>();

			services.AddTransient<IListPartServiceFactory, DefaultListPartServiceFactory>();
			services.AddTransient<IListPartService, DefaultListPartService>();

			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<UsersPermissionProvider>>();

			services.AddScoped<UserSessionContext>();
			services.AddScoped<IUserSessionContextSessionContextProvider, DefaultUserSessionContextSessionContextProvider>();

			return services;
		}
	}
}

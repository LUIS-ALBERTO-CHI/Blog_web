using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Users.AdminState.Parts;
using FwaEu.Modules.Users.CultureSettings.Parts;
using FwaEu.Modules.Users.CultureSettings.Parts.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.AdminState
{
	public static class AdminStateExtensions
	{
		public static IServiceCollection AddFwameworkModuleAdminState(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			services.AddTransient<IPartHandler, AdminStatePartHandler>();

			return services;
		}
	}
}

using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Users.CultureSettings.Parts;
using FwaEu.Modules.Users.CultureSettings.Parts.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.CultureSettings
{
	public static class CultureSettingsExtensions
	{
		public static IServiceCollection AddFwameworkModuleCultureSettings(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserCultureSettingsEntityRepository>();

			services.AddTransient<IPartHandler, CultureSettingsPartHandler>();

			services.AddTransient<IUserCultureSettingsProvider, EntityUserCultureSettingsProvider>();

			return services;
		}
	}
}

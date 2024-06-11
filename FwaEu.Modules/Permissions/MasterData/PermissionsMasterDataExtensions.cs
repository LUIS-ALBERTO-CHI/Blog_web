using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Permissions.MasterData
{
	public static class PermissionsMasterDataExtensions
	{
		public static IServiceCollection AddFwameworkModulePermissionsMasterData(this IServiceCollection services)
		{
			services.AddMasterDataProvider<PermissionMasterDataProvider>("Permissions");
			services.AddTransient<IPermissionMasterDataDate, EntityPermissionMasterDataDate>();

			return services;
		}
	}
}
		
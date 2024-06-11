using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericImporter.DataAccess;
using FwaEu.Modules.MasterData;
using FwaEu.TemplateCore.FarmManager.GenericAdmin;
using FwaEu.TemplateCore.FarmManager.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.Photo
{
	public static class FarmManagerPhotoExtensions
	{
		public static IServiceCollection AddApplicationFarmManagerPhoto(this IServiceCollection services,
				ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Application:FarmManager:Photos");
			services.Configure<PhotosOptions>(section);

			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<FarmPhotoEntityRepository>();

			services.AddTransient<IFarmPhotoFileCopyProcessFactory, DefaultFarmPhotoFileCopyProcessFactory>();
			services.AddTransient<IFarmPhotoService, DefaultFarmPhotoService>();

			return services;
		}
	}
}

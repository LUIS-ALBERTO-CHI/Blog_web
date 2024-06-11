using FwaEu.Modules.GenericImporter;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace FwaEu.Modules.MasterDataImporter
{
	public static class MasterDataImporterExtensions
	{
		public static IServiceCollection AddFwameworkModuleMasterDataImporter(this IServiceCollection services)
		{
			services.AddTransient<IModelImporterEventListenerFactory<IEntityImporterEventListener>, EntityMasterDataImporterEventListenerFactory>();

			return services;
		}
	}
}

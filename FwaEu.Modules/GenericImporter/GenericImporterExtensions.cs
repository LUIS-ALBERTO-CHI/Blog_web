using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public static class GenericImporterExtensions
	{
		public static IServiceCollection AddFwameworkModuleGenericImporter(this IServiceCollection services)
		{
			services.AddTransient(typeof(IModelImporter<>), typeof(EntityImporter<>));
			services.AddTransient(typeof(IModelBinder<>), typeof(DefaultModelBinder<>));
			services.AddTransient(typeof(IEntityDataAccessFactory<>), typeof(EntityDataAccessFactory<>));
			services.AddTransient(typeof(IDataAccessFactory<>), typeof(CacheEntityDataAccessFactory<>));

			services.AddTransient<IPropertyBinder, DefaultPropertyBinder>();
			services.AddTransient<IModelImporterEventListenerFactory<IEntityImporterEventListener>, EntityImporterEventListenerFactory>();

			return services;
		}
	}
}

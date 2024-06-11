using FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public static class DatabaseExtensions
	{
		public static IServiceCollection AddFwameworkDatabase(this IServiceCollection services)
		{
			services.AddTransient<IRepositoryFactory, RepositoryFactory>();
			services.AddSingleton<IInterceptorFactory, CompositeDispatchInterceptorFactory>();
			services.AddTransient(typeof(IEntityKeyResolver<>), typeof(PluralizationEntityResolver<>));

			return services;
		}
	}
}

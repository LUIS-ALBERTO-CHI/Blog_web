using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace FwaEu.TestTools.InMemoryDatabase.Sqlite
{
	public class InMemoryDatabaseCreationOptions
	{
		public InMemoryDatabaseCreationOptions(
			Action<IRepositoryRegister> addRepositoriesAction,
			params Type[] mappingTypes)
		{
			this.AddRepositoriesAction = addRepositoriesAction
				?? throw new ArgumentNullException(nameof(addRepositoriesAction));

			this.MappingTypes = mappingTypes
				?? throw new ArgumentNullException(nameof(mappingTypes));
		}

		public Action<IRepositoryRegister> AddRepositoriesAction { get; }
		public Type[] MappingTypes { get; }
	}

	public static class DependencyInjectionExtensions
	{
		public static ServiceCollection AddInMemoryDatabaseServices(this ServiceCollection services,
			InMemoryDatabaseCreationOptions options)
		{
			var connection = new SQLiteConnection(InMemorySessionFactoryProviderMock.MemorySharedConnectionString);

			services.AddSingleton(connection);
			services.AddSingleton<ISessionFactoryProvider>(new InMemorySessionFactoryProviderMock(options, connection));

			services.AddScoped<ISessionAdapterFactory, SessionAdapterFactory>();
			services.AddSingleton<IInterceptorFactory, CompositeDispatchInterceptorFactory>();

			services.AddScoped(serviceProvider => (INhibernateSessionAdapterFactory)
				serviceProvider.GetService<ISessionAdapterFactory>());

			services.AddScoped<IRepositoryFactory, RepositoryFactory>();

			services.AddStatelessSessionResolver<IStatelessSessionAdapter, ISessionAdapterFactory>();
			services.AddStatefulSessionResolver<IStatefulSessionAdapter, ISessionAdapterFactory>();

			services.AddScoped(typeof(IRepositorySessionFactory<>), typeof(DefaultRepositorySessionFactory<>));
			services.AddScoped<MainSessionContext>();

			var repositoryRegister = new RepositoryRegister();
			options.AddRepositoriesAction(repositoryRegister);
			services.AddSingleton<IRepositoryRegister>(repositoryRegister);

			return services;
		}
	}
}

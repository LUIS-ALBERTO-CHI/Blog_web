using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace FwaEu.TestTools.InMemoryDatabase.Sqlite
{
	public interface IDataScope
	{
		ServiceProvider ServiceProvider { get; }
		ISessionAdapter Session { get; }
	}
	public static class InMemoryDataScopeFactory
	{
		private static TDataScope CreateInMemoryDataScope<TDataScope, TSessionAdapter>(
			ServiceCollection services,
			InMemoryDatabaseCreationOptions options,
			Func<ISessionAdapterFactory, ServiceProvider, SQLiteConnection, TDataScope> factory)
				where TDataScope : InMemoryDataScope<TSessionAdapter>
				where TSessionAdapter : ISessionAdapter
		{
			services.AddInMemoryDatabaseServices(options);

			var serviceProvider = services.BuildServiceProvider();
			var sessionAdapterFactory = serviceProvider.GetRequiredService<ISessionAdapterFactory>();
			var connection = serviceProvider.GetRequiredService<SQLiteConnection>();

			return factory(sessionAdapterFactory, serviceProvider, connection);
		}

		public static StatelessInMemoryDataScope CreateStatelessInMemoryDataScope(
			ServiceCollection services,
			InMemoryDatabaseCreationOptions options)
		{
			return CreateInMemoryDataScope<StatelessInMemoryDataScope, IStatelessSessionAdapter>(
				services, options, (sessionAdapterFactory, serviceProvider, connection) =>
				{
					var session = sessionAdapterFactory.CreateStatelessSession(
						new CreateSessionOptions()
						{
							Connection = connection,
						});

					return new StatelessInMemoryDataScope(serviceProvider, session);
				});
		}

		public static StatefulInMemoryDataScope CreateStatefulInMemoryDataScope(
			ServiceCollection services,
			InMemoryDatabaseCreationOptions options)
		{
			return CreateInMemoryDataScope<StatefulInMemoryDataScope, IStatefulSessionAdapter>(
				services, options, (sessionAdapterFactory, serviceProvider, connection) =>
				{
					var session = sessionAdapterFactory.CreateStatefulSession(
						new CreateSessionOptions()
						{
							Connection = connection,
						});

					return new StatefulInMemoryDataScope(serviceProvider, session);
				});
		}
	}

	public abstract class InMemoryDataScope<TSessionAdapter> : IDisposable, IDataScope
		where TSessionAdapter : ISessionAdapter
	{
		protected InMemoryDataScope(ServiceProvider serviceProvider, TSessionAdapter session)
		{
			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this.Session = session;
		}

		public ServiceProvider ServiceProvider { get; }
		public TSessionAdapter Session { get; }
		ISessionAdapter IDataScope.Session { get { return Session; } }

		public void Dispose()
		{
			this.Session.Dispose();
			this.ServiceProvider.Dispose();
		}
	}

	public class StatelessInMemoryDataScope : InMemoryDataScope<IStatelessSessionAdapter>
	{
		public StatelessInMemoryDataScope(ServiceProvider serviceProvider, IStatelessSessionAdapter session)
			: base(serviceProvider, session)
		{
		}
	}

	public class StatefulInMemoryDataScope : InMemoryDataScope<IStatefulSessionAdapter>
	{
		public StatefulInMemoryDataScope(ServiceProvider serviceProvider, IStatefulSessionAdapter session)
			: base(serviceProvider, session)
		{
		}
	}
}

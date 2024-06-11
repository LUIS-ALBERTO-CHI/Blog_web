using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Fwamework.Data.Database.Nhibernate
{
	public interface ISessionFactoryProvider
	{
		ISessionFactory GetFactory(CreateSessionOptions options);
		DefaultConnectionInfo DefaultConnectionInfo { get; }
	}

	public class DefaultConnectionInfo
	{
		public DefaultConnectionInfo(string connectionStringName, Type databaseFeaturesType)
		{
			this.ConnectionStringName = connectionStringName
				?? throw new ArgumentNullException(nameof(connectionStringName));

			this.DatabaseFeaturesType = databaseFeaturesType
				?? throw new ArgumentNullException(nameof(databaseFeaturesType));
		}

		public string ConnectionStringName { get; }
		public Type DatabaseFeaturesType { get; }
	}

	public class SessionFactoryProvider : ISessionFactoryProvider
	{
		public SessionFactoryProvider(IEnumerable<INhibernateConfigurationLoader> configurationLoaders)
		{
			var loaders = configurationLoaders.ToList();

			if (loaders.Count > 0)
			{
				var firstLoader = loaders.FirstOrDefault(x => x.ConnectionStringName.Equals("Default", StringComparison.InvariantCultureIgnoreCase)) ?? loaders.First();
				this.DefaultConnectionInfo = new DefaultConnectionInfo(firstLoader.ConnectionStringName, firstLoader.DatabaseFeaturesType);
			}

			this._loadedFactories = new Dictionary<string, ISessionFactory>(loaders.Count);
			this._loaders = loaders;
		}

		public DefaultConnectionInfo DefaultConnectionInfo { get; }
		private readonly Dictionary<string, ISessionFactory> _loadedFactories;
		private List<INhibernateConfigurationLoader> _loaders;

		public ISessionFactory GetFactory(CreateSessionOptions options)
		{
			var effectiveConnectionStringName = options?.ConnectionStringName
				?? this.DefaultConnectionInfo.ConnectionStringName;

			if (!this._loadedFactories.ContainsKey(effectiveConnectionStringName))
			{
				lock (this)
				{
					if (!this._loadedFactories.ContainsKey(effectiveConnectionStringName))
					{
						var loader = GetLoader(effectiveConnectionStringName, this._loaders);
						this._loadedFactories.Add(effectiveConnectionStringName, loader.Load().BuildSessionFactory());
						this._loaders.Remove(loader);

						if (!this._loaders.Any())
						{
							this._loaders = null;
						}
					}
				}
			}

			return this._loadedFactories[effectiveConnectionStringName];
		}

		private static INhibernateConfigurationLoader GetLoader(string connectionStringName, List<INhibernateConfigurationLoader> loaders)
		{
			var loader = default(INhibernateConfigurationLoader);
			if (loaders == null || (loader = loaders.FirstOrDefault(l => l.ConnectionStringName == connectionStringName)) == null)
			{
				throw new ConnectionStringNotConfiguredException(
					$"The connection string name '{connectionStringName}' is not configured in application Startup.");
			}
			return loader;
		}
	}
}

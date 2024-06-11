using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Data.Database.Sessions
{
	[TestClass]
	public class DefaultRepositorySessionFactoryTests
	{
		private static ServiceCollection CreateServiceCollection()
		{
			var services = new ServiceCollection();

			services.AddInMemoryDatabaseServices(
				new InMemoryDatabaseCreationOptions((repositories) => { }));

			services.AddFwameworkSessionsServices();

			return services;
		}

		[TestMethod]
		public void CreateSession_IStatefulSessionAdapter()
		{
			var services = CreateServiceCollection();

			using (var serviceProvider = services.BuildServiceProvider())
			{
				var sessionFactory = serviceProvider.GetRequiredService<IRepositorySessionFactory<IStatefulSessionAdapter>>();
				using (var repositorySession = sessionFactory.CreateSession())
				{
					Assert.IsNotNull(repositorySession.Session);
				}
			}
		}

		[TestMethod]
		public void CreateSession_INhibernateStatefulSessionAdapter()
		{
			var services = CreateServiceCollection();
			NhibernateExtensions.AddSessionResolvers(services);

			using (var serviceProvider = services.BuildServiceProvider())
			{
				var sessionFactory = serviceProvider.GetRequiredService<IRepositorySessionFactory<INhibernateStatefulSessionAdapter>>();
				using (var repositorySession = sessionFactory.CreateSession())
				{
					Assert.IsNotNull(repositorySession.Session);
					
					Assert.AreEqual(
						InMemorySessionFactoryProviderMock.MemorySharedConnectionString,
						repositorySession.Session.NhibernateSession.Connection.ConnectionString);
					
				}
			}
		}
	}
}

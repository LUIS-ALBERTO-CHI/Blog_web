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
	public class DefaultRepositorySessionTests
	{
		private static ServiceCollection CreateServiceCollection()
		{
			var services = new ServiceCollection();

			services.AddInMemoryDatabaseServices(
				new InMemoryDatabaseCreationOptions((repositories) =>
				{
					repositories.Add<RepositoryDummy>();
				}));

			services.AddFwameworkSessionsServices();

			return services;
		}

		[TestMethod]
		public void CreateSession_IStatefulSessionAdapter()
		{
			var services = CreateServiceCollection();

			using (var serviceProvider = services.BuildServiceProvider())
			{
				var sessionFactory = serviceProvider
					.GetRequiredService<IRepositorySessionFactory<IStatefulSessionAdapter>>();
				using (var repositorySession = sessionFactory.CreateSession())
				{
					var repository = repositorySession.Create<RepositoryDummy>();
					Assert.IsInstanceOfType(repository, typeof(RepositoryDummy));
				}
			}
		}
	}
}

using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Monitoring
{
	public class DefaultMonitoringService : IMonitoringService
	{

		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;

		public DefaultMonitoringService(ISessionAdapterFactory sessionAdapterFactory, IRepositoryFactory repositoryFactory)
		{
			_sessionAdapterFactory = sessionAdapterFactory ?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
			_repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
		}

		public async Task PingAsync()
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var userRepository = this._repositoryFactory.Create<IUserEntityRepository>(session);
				await userRepository.Query().AnyAsync(u => u.Id == 0);
			}
		}
	}
}

using FluentNHibernate.Data;
using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.ReportsMasterDataByEntities;
using MimeKit.Encodings;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{

	public interface IReportFiltersDataService
	{
		Task<ReportDataModel> LoadDataAsync(string filterInvariantId, CancellationToken cancellationToken);
	}

	public class DefaultReportFiltersDataService : IReportFiltersDataService
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly IDataService _dataService;
		private readonly IParametersService _parametersService;

		public DefaultReportFiltersDataService(ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory,
			IDataService dataService,
			IParametersService parametersService)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));

			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));

			this._dataService = dataService
				?? throw new ArgumentNullException(nameof(dataService));

			this._parametersService = parametersService
				?? throw new ArgumentNullException(nameof(parametersService));
		}

		public async Task<ReportDataModel> LoadDataAsync(string filterInvariantId, CancellationToken cancellationToken)
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportFilterEntityRepository>(session);
				var filter = await repository.GetByInvariantIdAsync(filterInvariantId);

				if (filter == null)
				{
					throw new NotFoundException($"Filter not found with invariant id: {filterInvariantId}.");
				}

				var parameters = await this._parametersService.LoadParametersAsync();

				return await this._dataService
					.LoadDataScope(new ReportDataSource(filter.DataSourceType, filter.DataSourceArgument), parameters)
					.LoadDataTask(cancellationToken);
			}
		}
	}
}

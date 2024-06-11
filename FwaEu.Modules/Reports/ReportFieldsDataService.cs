using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.ReportsMasterDataByEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportFieldsDataService
	{
		Task<ReportDataModel> LoadDataAsync(string fieldInvariantId, CancellationToken cancellationToken);
	}

	public class DefaultReportFieldsDataService : IReportFieldsDataService
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly IDataService _dataService;
		private readonly IParametersService _parametersService;

		public DefaultReportFieldsDataService(ISessionAdapterFactory sessionAdapterFactory,
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

		public async Task<ReportDataModel> LoadDataAsync(string fieldInvariantId, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportFieldEntityRepository>(session);
				var field = await repository.GetByInvariantIdAsync(fieldInvariantId);

				if (field == null)
				{
					throw new NotFoundException($"Field not found with invariant id: {fieldInvariantId}.");
				}

				var parameters = await this._parametersService.LoadParametersAsync();

				return await this._dataService
					.LoadDataScope(new ReportDataSource(field.DataSourceType, field.DataSourceArgument), parameters)
					.LoadDataTask(cancellationToken);
			}
		}
	}
}

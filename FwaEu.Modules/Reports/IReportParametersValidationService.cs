using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportParametersValidationService
	{
		Task ValidateAsync(ParametersModel parameters, ReportLoadResultModel<ServiceReportModel> model);
	}

	public class ReportParametersValidationService : IReportParametersValidationService
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly IReportFilterDataProvider _repositoryFilterDataProvider;

		public ReportParametersValidationService(ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory, IReportFilterDataProvider repositoryFilterDataProvider)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));
			this._repositoryFilterDataProvider = repositoryFilterDataProvider
				?? throw new ArgumentNullException(nameof(repositoryFilterDataProvider));
		}

		public async Task ValidateAsync(ParametersModel parameters, ReportLoadResultModel<ServiceReportModel> model)
		{
			var reportModels = await _repositoryFilterDataProvider.GetModelsAsync();
			var filtersInvariantIds = model.Report.Filters.Select(f => f.InvariantId).ToArray();
			var missingReports = filtersInvariantIds.Except(reportModels.Select(x => x.InvariantId)).ToArray();

			var modelsUsedInThisReport = reportModels.Where(x => filtersInvariantIds.Contains(x.InvariantId)).ToDictionary(x => x.InvariantId);

			foreach (var filter in model.Report.Filters)
			{
				if (parameters.Parameters.ContainsKey(filter.InvariantId))
				{
					var value = parameters.Parameters[filter.InvariantId];
					var type = modelsUsedInThisReport[filter.InvariantId].DotNetTypeName;
					var valueType = value.GetType();
					var @namespace = valueType.Namespace + ".";

					if (!valueType.IsAssignableFrom(Type.GetType(@namespace + type)))
					{
						throw new BadParametersInFilterException(String.Format("Wrong value type for parameter: {0}. " +
							"Value must be of type: {1}.", filter.InvariantId, type));
					}
				}
				else if (filter.IsRequired)
				{
					throw new BadParametersInFilterException(
						String.Format("Missing value for required parameter: {0}.", filter.InvariantId));
				}
			}
		}
	}
	public class BadParametersInFilterException : Exception
	{
		public BadParametersInFilterException(string message)
			: base(message)
		{

		}
	}
}
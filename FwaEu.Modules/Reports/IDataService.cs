using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Reports.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IDataService
	{
		DataServiceDataScope LoadDataScope(ReportDataSource dataSource, ParametersModel parameters);
	}

	public class DataServiceDataScope
	{
		public DataServiceDataScope(
			Func<CancellationToken, Task<ReportDataModel>> loadDataTask,
			IReadOnlyDictionary<string, object> logScope)
		{
			this.LoadDataTask = loadDataTask ?? throw new ArgumentNullException(nameof(loadDataTask));
			this.LogScope = logScope ?? throw new ArgumentNullException(nameof(logScope));
		}

		public Func<CancellationToken, Task<ReportDataModel>> LoadDataTask { get; }
		public IReadOnlyDictionary<string, object> LogScope
		{
			get;
		}
	}

	public class DefaultDataService : IDataService
	{
		protected IEnumerable<IReportDataProviderFactory> Factories { get; }
		protected IServiceProvider ServiceProvider { get; }

		public DefaultDataService(
		IEnumerable<IReportDataProviderFactory> factories,
		IServiceProvider serviceProvider)
		{
			this.Factories = factories
				?? throw new ArgumentNullException(nameof(factories));

			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		protected virtual IReportDataProvider CreateProvider(string dataSourceType)
		{
			return this.Factories
				.Select(factory => factory.Create(dataSourceType, this.ServiceProvider))
				.FirstOrDefault(provider => provider != null);
		}

		public DataServiceDataScope LoadDataScope(ReportDataSource dataSource, ParametersModel parameters)
		{
			var provider = this.CreateProvider(dataSource.Type);

			if (provider == null)
			{
				throw new ArgumentException(
					$"No provider found for data source type: {dataSource.Type}.",
					nameof(dataSource.Type));
			}

			//TODO: Validate Filters, ".net type matching with IParametersService values" https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/5167

			var logScope = provider.GetLogScope(dataSource.Argument);
			return new DataServiceDataScope(
				async (CancellationToken cancellationToken) =>
				{
					cancellationToken.ThrowIfCancellationRequested();
					return await provider.LoadDataAsync(dataSource.Argument, parameters, cancellationToken);
				}, logScope);
		}
	}
}

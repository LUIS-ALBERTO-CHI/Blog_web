using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Modules.Reports
{
	public interface IReportDataProviderFactory
	{
		IReportDataProvider Create(string dataSourceType, IServiceProvider serviceProvider);
	}

	public class DefaultReportDataProviderFactory<TProvider> : IReportDataProviderFactory
		where TProvider : IReportDataProvider
	{
		public DefaultReportDataProviderFactory(string dataSourceType)
		{
			this.DataSourceType = dataSourceType
				?? throw new ArgumentNullException(nameof(dataSourceType));
		}

		public string DataSourceType { get; }

		public IReportDataProvider Create(string dataSourceType, IServiceProvider serviceProvider)
		{
			if (dataSourceType == this.DataSourceType)
			{
				return serviceProvider.GetService<TProvider>();
			}

			return null;
		}
	}

	public interface IReportDataProvider
	{
		Task<ReportDataModel> LoadDataAsync(
			string dataSourceArgument,
			ParametersModel parameters,
			CancellationToken cancellationToken);

		IReadOnlyDictionary<string, object> GetLogScope(string dataSourceArgument);
	}

	public class ReportDataModel
	{
		public ReportDataModel(Dictionary<string, object>[] rows)
		{
			this.Rows = rows
				?? throw new ArgumentNullException(nameof(rows));
		}
		public Dictionary<string, object>[] Rows { get; }
	}
}

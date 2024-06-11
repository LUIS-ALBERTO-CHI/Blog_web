using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;


namespace FwaEu.Modules.Reports
{
	public class ClientSideCustomReportDataProvider : IReportDataProvider
	{
		public class ClientSideCustomParameters
		{
			public string Text { get; set; }
			public bool GetWordLength { get; set; }
		}
		public ClientSideCustomReportDataProvider()
		{

		}

		public IReadOnlyDictionary<string, object> GetLogScope(string dataSourceArgument)
		{
			return new Dictionary<string, object>();			
		}

		public Task<ReportDataModel> LoadDataAsync(
			string dataSourceArgument,
			ParametersModel parameters,
			CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			var arguments = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(dataSourceArgument);

			var words = Convert.ToString(arguments["text"]).Split(" ");
			var getWordLength = Convert.ToBoolean(arguments["getWordLength"]);
			var getWordOrder = Convert.ToBoolean(arguments["getWordOrder"]);
			var rowsList = new List<Dictionary<string, object>>();
			var i = 0;
			foreach(var word in words)
			{
				i++;
				var row = new Dictionary<string, object>
				{
					["Word"] = word,
				};
				if (getWordLength)
					row.Add("Length", word.Length);
				if (getWordOrder)
					row.Add("Order", i);

				rowsList.Add(row);
			}

			return Task.FromResult<ReportDataModel>(new ReportDataModel(rowsList.ToArray()));
		}
	}

	public class ClientSideCustomReportDataProviderFactory : IReportDataProviderFactory
	{
		public IReportDataProvider Create(string dataSourceType, IServiceProvider serviceProvider)
		{
			if (dataSourceType == "ClientSideCustom")
			{
				return serviceProvider.GetService<ClientSideCustomReportDataProvider>();
			}
			return null;
		}
	}
}

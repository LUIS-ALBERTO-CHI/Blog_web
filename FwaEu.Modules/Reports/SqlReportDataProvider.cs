using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Transform;

namespace FwaEu.Modules.Reports
{
	public class SqlReportDataProvider : IReportDataProvider
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;

		public SqlReportDataProvider(ISessionAdapterFactory sessionAdapterFactory)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
		}

		public IReadOnlyDictionary<string, object> GetLogScope(string dataSourceArgument)
		{
			var parameters = new Dictionary<string, object>
			{
				["Sql"] = dataSourceArgument
			};
			return parameters;
		}

		public async Task<ReportDataModel> LoadDataAsync(
			string dataSourceArgument,
			ParametersModel parameters,
			CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			using (var session = (INhibernateStatefulSessionAdapter)_sessionAdapterFactory.CreateStatefulSession())
			{
				var query = session.NhibernateSession.CreateSQLQuery(dataSourceArgument);
				var parametersDictionary = parameters.Parameters;

				foreach (var queryParameter in query.NamedParameters)
				{
					query.SetParameter(queryParameter,
						parametersDictionary.ContainsKey(queryParameter)
							? parametersDictionary[queryParameter] : null);
				}

				var rowsList = await query
					.SetResultTransformer(new ReportDataModelTransformer())
					.ListAsync<Dictionary<string, object>>(cancellationToken);

				return new ReportDataModel(rowsList.ToArray());
			}
		}
	}

	public class SqlReportDataProviderFactory : IReportDataProviderFactory
	{
		public IReportDataProvider Create(string dataSourceType, IServiceProvider serviceProvider)
		{
			if (dataSourceType == "Sql")
			{
				return serviceProvider.GetService<SqlReportDataProvider>();
			}
			return null;
		}
	}

	public class ReportDataModelTransformer : IResultTransformer
	{
		public IList TransformList(IList collection)
		{
			return collection;
		}

		public object TransformTuple(object[] tuple, string[] aliases)
		{
			var pascalCaseAliases = aliases.Select(x => x.Split(new[] { "_" },
				StringSplitOptions.RemoveEmptyEntries)
			.Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
			.Aggregate(string.Empty, (s1, s2) => s1 + s2)).ToArray();

			var result = new Dictionary<string, object>();
			for (int i = 0; i < pascalCaseAliases.Length; i++)
			{
				result[pascalCaseAliases[i]] = tuple[i];
			}
			return result;
		}
	}
}

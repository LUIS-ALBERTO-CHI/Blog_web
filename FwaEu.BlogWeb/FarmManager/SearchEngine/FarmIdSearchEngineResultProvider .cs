using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.SearchEngine;
using FwaEu.BlogWeb.FarmManager.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.SearchEngine
{
	public class FarmIdSearchEngineResultProvider : ISearchEngineResultProvider
	{
		private readonly MainSessionContext _sessionContext;

		public FarmIdSearchEngineResultProvider(MainSessionContext sessionContext)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public async Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			if (Int32.TryParse(search, out int id))
			{
				var models = await this._sessionContext.RepositorySession
					.Create<FarmEntityRepository>()
					.Query()
					.Where(f => f.ToString().Contains(id.ToString()))
					.SkipTake(pagination)
					.OrderBy(f => f.Id != id) // NOTE: Will put the exact match as first result
					.ThenBy(f => f.Id)
					.Select(f => new
					{
						f.Id,
						f.Name,
						PostalCodeId = f.PostalCode.Id,
					})
					.ToListAsync();

				return models;
			}

			return Enumerable.Empty<object>();
		}
	}
}

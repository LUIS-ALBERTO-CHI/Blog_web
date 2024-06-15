using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.SearchEngine;
using FwaEu.BlogWeb.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.SearchEngine
{
	public class FarmerSearchEngineResultProvider : ISearchEngineResultProvider
	{
		private readonly MainSessionContext _sessionContext;

		public FarmerSearchEngineResultProvider(MainSessionContext sessionContext)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public async Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			var models = await this._sessionContext.RepositorySession
				.Create<ApplicationUserEntityRepository>()
				.Query()
				.Where(u => u.FarmerPseudonym.Contains(search))
				.SkipTake(pagination)
				.Select(u => new
				{
					u.Id,
					u.FarmerPseudonym,
				})
				.ToListAsync();

			return models;
		}
	}
}

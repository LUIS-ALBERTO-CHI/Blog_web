using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.SearchEngine;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.SearchEngine
{
	public class UserIdSearchEngineResultProvider : ISearchEngineResultProvider
	{
		private readonly MainSessionContext _sessionContext;

		public UserIdSearchEngineResultProvider(MainSessionContext sessionContext)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public async Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			if (Int32.TryParse(search, out int id))
			{
				var models = await this._sessionContext.RepositorySession
					.Create<ApplicationUserEntityRepository>()
					.Query()
					.Where(u => u.Id.ToString().Contains(id.ToString()))
					.SkipTake(pagination)
					.OrderBy(u => u.Id != id) // NOTE: Will put the exact match as first result
					.ThenBy(u => u.Id)
					.Select(u => new
					{
						u.Id,
						u.FirstName,
						u.LastName,
						u.Identity,
					})
					.ToListAsync();

				return models;
			}

			return Enumerable.Empty<object>();
		}
	}
}

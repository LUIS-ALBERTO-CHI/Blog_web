using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.SearchEngine;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users.SearchEngine
{
	public class UserSearchEngineResultProvider : ISearchEngineResultProvider
	{
		private readonly MainSessionContext _sessionContext;

		public UserSearchEngineResultProvider(MainSessionContext sessionContext)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public async Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			var models = await this._sessionContext.RepositorySession
				.Create<ApplicationUserEntityRepository>()
				.Query()
				.Where(u => (u.FirstName + " " + u.LastName).Contains(search) || u.Identity.Contains(search))
				.SkipTake(pagination)
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
	}
}

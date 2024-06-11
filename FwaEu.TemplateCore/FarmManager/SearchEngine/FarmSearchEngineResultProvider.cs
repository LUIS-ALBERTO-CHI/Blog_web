using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.SearchEngine;
using FwaEu.TemplateCore.FarmManager.Entities;
using FwaEu.TemplateCore.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.SearchEngine
{
	public class FarmSearchEngineResultProvider : ISearchEngineResultProvider
	{
		private readonly MainSessionContext _sessionContext;

		public FarmSearchEngineResultProvider(MainSessionContext sessionContext)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public async Task<IEnumerable<object>> SearchAsync(string search, SearchPagination pagination)
		{
			var models = await this._sessionContext.RepositorySession
				.Create<FarmEntityRepository>()
				.Query()
				.Where(f => f.Name.Contains(search))
				.SkipTake(pagination)
				.Select(f => new
				{
					f.Id,
					f.Name,
					PostalCodeId = f.PostalCode.Id,
				})
				.ToListAsync();

			return models;
		}
	}
}

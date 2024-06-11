using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.UserTasks;
using FwaEu.TemplateCore.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.UserTasks
{
	public class FarmCountResult
	{
		public FarmCountResult(int count)
		{
			this.Count = count;
		}

		public int Count { get; }
	}

	public class FarmerCountUserTask : UserTask<FarmCountResult>
	{
		private readonly MainSessionContext _mainSessionContext;

		public FarmerCountUserTask(MainSessionContext mainSessionContext)
		{
			this._mainSessionContext = mainSessionContext
				?? throw new ArgumentNullException(nameof(mainSessionContext));
		}

		public override async Task<FarmCountResult> ExecuteAsync(CancellationToken cancellationToken)
		{
			var count = await this._mainSessionContext.RepositorySession
				.Create<ApplicationUserEntityRepository>()
				.Query()
				.Where(user => user.FarmerPseudonym != null
					&& user.FarmerPseudonym != String.Empty)
				.CountAsync();

			return new FarmCountResult(count);
		}
	}
}

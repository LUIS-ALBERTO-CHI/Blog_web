using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.UserTasks;
using FwaEu.TemplateCore.FarmManager.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.UserTasks
{
	public class FarmWithoutAnimalsParameters
	{
		public int? ActivityId { get; set; }
	}

	public class FarmWithoutAnimalsResult
	{
		public FarmWithoutAnimalsResult(int count, string type)
		{
			Count = count;
			Type = type;
		}

		public int Count { get; set; }
		public string Type { get; set; }
	}

	public class FarmsWithoutAnimalsUserTask : UserTask<FarmWithoutAnimalsParameters, FarmWithoutAnimalsResult>
	{
		private readonly MainSessionContext _mainSessionContext;

		public FarmsWithoutAnimalsUserTask(MainSessionContext mainSessionContext)
		{
			this._mainSessionContext = mainSessionContext
				?? throw new ArgumentNullException(nameof(mainSessionContext));
		}

		public override async Task<FarmWithoutAnimalsResult> ExecuteAsync(
			FarmWithoutAnimalsParameters parameters,
			CancellationToken cancellationToken)
		{
			var farmAnimalCountQuery = this._mainSessionContext.RepositorySession
				.Create<FarmAnimalCountEntityRepository>()
				.Query();

			var farmsWithoutAnimalsQuery = this._mainSessionContext.RepositorySession
				.Create<FarmEntityRepository>()
				.Query()
				.Where(farm => !farmAnimalCountQuery.Any(fac => fac.Farm == farm));

			if (parameters.ActivityId.HasValue)
				farmsWithoutAnimalsQuery = farmsWithoutAnimalsQuery.Where(farm => farm.MainActivity.Id == parameters.ActivityId.Value);

			var count = await farmsWithoutAnimalsQuery
				.CountAsync();

			return new FarmWithoutAnimalsResult(count, "warning");
		}
	}
}

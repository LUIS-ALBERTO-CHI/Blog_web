using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.HistoryPart.Services
{
	public class HistoryPartHandler : ReadOnlyPartHandler<HistoryPartModel>
	{
		private readonly UserSessionContext _userSessionContext;

		public const string PartName = "History";
		public override string Name => PartName;

		public HistoryPartHandler(UserSessionContext userSessionContext)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));
		}

		public override Task<HistoryPartModel> LoadAsync()
		{
			var loadingModel = (IHistoryPartLoadingModelPropertiesAccessor)this._userSessionContext.LoadingModel;

			return Task.FromResult(new HistoryPartModel()
			{
				CreatedById = loadingModel.CreatedById,
				CreatedOn = loadingModel.CreatedOn,
				UpdatedById = loadingModel.UpdatedById,
				UpdatedOn = loadingModel.UpdatedOn,
			});
		}
	}
}

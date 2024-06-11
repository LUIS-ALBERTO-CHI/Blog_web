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
	public class ListHistoryPartHandler : ListPartHandler<ListHistoryPartModel>
	{
		public override string Name => HistoryPartHandler.PartName;

		public override Task<ListHistoryPartModel> LoadAsync(UserListModel model)
		{
			var loadingModel = (IUpdateHistoryPartLoadingModelPropertiesAccessor)model;

			return Task.FromResult(new ListHistoryPartModel()
			{
				UpdatedById = loadingModel.UpdatedById,
				UpdatedOn = loadingModel.UpdatedOn,
			});
		}
	}
}

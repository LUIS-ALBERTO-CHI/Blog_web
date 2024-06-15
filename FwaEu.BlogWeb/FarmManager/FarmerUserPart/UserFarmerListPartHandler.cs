using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.FarmerUserPart
{
	public class UserFarmerListModel
	{
		public string Pseudonym { get; set; }
	}

	public class UserFarmerListPartHandler : ListPartHandler<UserFarmerListModel>
	{
		public override string Name => "Farmer";

		public override Task<UserFarmerListModel> LoadAsync(UserListModel model)
		{
			var accessor = (IFarmerPartLoadingListModelPropertiesAccessor)model;
			
			var part = new UserFarmerListModel()
			{
				Pseudonym =  accessor.FarmerPseudonym,
			};

			return Task.FromResult(part);
		}
	}
}

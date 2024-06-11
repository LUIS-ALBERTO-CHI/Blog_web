using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.HistoryPart.Services;
using FwaEu.TemplateCore.FarmManager.FarmerUserPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users
{
	public class ApplicationUserLoadingListModel : FwaEu.Fwamework.Users.UserListModel
		, IUpdateHistoryPartLoadingModelPropertiesAccessor
		, IFarmerPartLoadingListModelPropertiesAccessor
		, IApplicationPartLoadingListModelPropertiesAccessor
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public UserState State { get; set; }
		public int? UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }

		public string FarmerPseudonym { get; set; }
	}
}
using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager
{
	public class FarmManagerPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanAccessToFarmManager { get; set; }
		public IPermission CanSaveFarms { get; set; }
		public IPermission CanDeleteFarms { get; set; }
		public IPermission CanAdministrateFarmMasterData { get; set; }
	}
}

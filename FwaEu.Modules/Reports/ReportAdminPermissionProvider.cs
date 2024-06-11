using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportAdminPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanAdministrateReports { get; set; }
	}
}

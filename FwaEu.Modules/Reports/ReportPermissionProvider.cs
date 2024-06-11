using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanViewReports { get; set; }
	}
}

using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Permissions
{
	public class PermissionProviderStub : ReflectedPermissionProvider
	{
		public IPermission CanAccessToReportingFake { get; set; }
		public IPermission CanDeleteUsersFake { get; set; }
	}
}

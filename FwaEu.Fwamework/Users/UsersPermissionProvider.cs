using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public class UsersPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanAdministrateUsers { get; set; }
	}
}

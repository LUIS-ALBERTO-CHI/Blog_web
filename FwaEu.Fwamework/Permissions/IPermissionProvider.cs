using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public interface IPermissionProvider
	{
		IEnumerable<IPermission> GetPermissions();
	}
}

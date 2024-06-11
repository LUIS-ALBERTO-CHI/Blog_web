using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public interface IPermissionManager
	{
		Task<bool> HasPermissionAsync(UserEntity user, IPermission permission);
	}
}

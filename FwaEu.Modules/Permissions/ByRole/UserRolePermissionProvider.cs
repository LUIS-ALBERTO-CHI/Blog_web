using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Permissions.ByRole
{
    public class UserRolePermissionProvider: ReflectedPermissionProvider
    {
        public IPermission CanAdministrateRoles { get; set; }
    }
}

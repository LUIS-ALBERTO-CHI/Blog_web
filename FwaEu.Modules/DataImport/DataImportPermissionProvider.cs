using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.DataImport
{
    public class DataImportPermissionProvider: ReflectedPermissionProvider
    {
        public IPermission CanImport { get; set; }
    }
}

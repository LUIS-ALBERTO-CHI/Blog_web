using FwaEu.Fwamework.Permissions;
using System;
using System.Collections.Generic;

namespace FwaEu.Modules.Currencies
{
	public class CurrenciesPermissionProvider : ReflectedPermissionProvider
	{
		public IPermission CanAdministrateCurrencies { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public interface IPermissionProviderAccessor<TProvider>
		where TProvider : IPermissionProvider
	{
		TProvider Provider { get; }
	}

	public class PermissionProviderAccessor<TProvider> : IPermissionProviderAccessor<TProvider>
		where TProvider : IPermissionProvider
	{
		public PermissionProviderAccessor(IPermissionProviderRegister permissionProviderRegister)
		{
			this._permissionProviderRegister = permissionProviderRegister;
		}

		private readonly IPermissionProviderRegister _permissionProviderRegister;

		public TProvider Provider => this._permissionProviderRegister.GetProviders().OfType<TProvider>().First();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public interface IPermissionProviderRegister
	{
		IEnumerable<IPermissionProvider> GetProviders();
	}

	public class PermissionProviderRegister : IPermissionProviderRegister
	{
		private IPermissionProvider[] _providers;
		private IEnumerable<IPermissionProviderFactory> _factories;

		public PermissionProviderRegister(IEnumerable<IPermissionProviderFactory> factories)
		{
			this._factories = factories ?? Enumerable.Empty<IPermissionProviderFactory>(); //NOTE: Case of an application without permissions
		}

		public IEnumerable<IPermissionProvider> GetProviders()
		{
			if (this._providers == null)
			{
				lock (this)
				{
					if (this._providers == null)
					{
						var providers = this._factories.Select(f => f.Create()).ToArray();
						Thread.MemoryBarrier();
						this._providers = providers;
						this._factories = null;
					}
				}
			}

			return this._providers;
		}
	}
}

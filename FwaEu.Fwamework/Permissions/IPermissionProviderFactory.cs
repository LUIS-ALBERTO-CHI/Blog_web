using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public interface IPermissionProviderFactory
	{
		IPermissionProvider Create();
	}

	public class DefaultPermissionProviderFactory<TProvider> : IPermissionProviderFactory
		where TProvider : IPermissionProvider, new()
	{
		public IPermissionProvider Create()
		{
			return new TProvider();
		}
	}
}

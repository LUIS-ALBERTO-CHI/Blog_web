using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public class SimplePermissionManager
	{
		public SimplePermissionManager(IPermissionManager permissionManager,
			IServiceProvider serviceProvider)
		{
			this.PermissionManager = permissionManager
				?? throw new ArgumentNullException(nameof(permissionManager));

			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public IPermissionManager PermissionManager { get; }
		protected IServiceProvider ServiceProvider { get; }

		public async Task<bool> HasPermissionAsync<TProvider>(UserEntity user, Func<TProvider, IPermission> getPermission)
			where TProvider : IPermissionProvider
		{
			var provider = this.ServiceProvider.GetRequiredService<IPermissionProviderAccessor<TProvider>>().Provider;
			var permission = getPermission(provider);

			return await this.PermissionManager.HasPermissionAsync(user, permission);
		}
	}
}

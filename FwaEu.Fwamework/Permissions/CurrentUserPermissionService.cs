using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions
{
	public class CurrentUserPermissionService
	{
		public ICurrentUserService CurrentUserService { get; }
		public SimplePermissionManager SimplePermissionManager { get; }

		public CurrentUserPermissionService(ICurrentUserService currentUserService,
			SimplePermissionManager simplePermissionManager)
		{
			this.CurrentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));

			this.SimplePermissionManager = simplePermissionManager
				?? throw new ArgumentNullException(nameof(simplePermissionManager));
		}

		public async Task<bool> HasPermissionAsync(IPermission permission)
		{
			return await this.SimplePermissionManager.PermissionManager.HasPermissionAsync(
				this.CurrentUserService.User?.Entity, permission);
		}

		public async Task<bool> HasPermissionAsync<TProvider>(Func<TProvider, IPermission> getPermission)
			where TProvider : IPermissionProvider
		{
			return await this.SimplePermissionManager.HasPermissionAsync<TProvider>(
				this.CurrentUserService.User?.Entity, getPermission);
		}
	}
}

using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SimpleMasterData.GenericAdmin
{
	public interface IGenericAdminSimpleMasterDataSecurityManager
	{
		Task<bool> IsAccessibleAsync();
	}

	public interface IGenericAdminSimpleMasterDataSecurityManagerFactory<TEntity>
		where TEntity : SimpleMasterDataEntityBase
	{
		IGenericAdminSimpleMasterDataSecurityManager CreateManager(IServiceProvider serviceProvider);
	}

	public class CustomGenericAdminSimpleMasterDataSecurityManagerFactory<TEntity, TManager>
		: IGenericAdminSimpleMasterDataSecurityManagerFactory<TEntity>
			where TEntity : SimpleMasterDataEntityBase
			where TManager : class, IGenericAdminSimpleMasterDataSecurityManager
	{
		public IGenericAdminSimpleMasterDataSecurityManager CreateManager(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<TManager>();
		}
	}

	public class CurrentUserPermissionGenericAdminSimpleMasterDataSecurityManager<TPermissionProvider> : IGenericAdminSimpleMasterDataSecurityManager
		where TPermissionProvider : IPermissionProvider
	{
		private readonly GetPermission<TPermissionProvider> _delegate;
		private readonly IPermissionProviderAccessor<TPermissionProvider> _permissionAccessor;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public CurrentUserPermissionGenericAdminSimpleMasterDataSecurityManager(
			GetPermission<TPermissionProvider> @delegate,
			IPermissionProviderAccessor<TPermissionProvider> permissionAccessor,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._delegate = @delegate
				?? throw new ArgumentNullException(nameof(@delegate));

			this._permissionAccessor = permissionAccessor
				?? throw new ArgumentNullException(nameof(permissionAccessor));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		public async Task<bool> IsAccessibleAsync()
		{
			var permission = this._delegate(this._permissionAccessor.Provider);
			return await this._currentUserPermissionService.HasPermissionAsync(permission);
		}
	}

	public class CurrentUserPermissionGenericAdminSimpleMasterDataSecurityManagerFactory<TEntity, TPermissionProvider>
	: IGenericAdminSimpleMasterDataSecurityManagerFactory<TEntity>
		where TEntity : SimpleMasterDataEntityBase
		where TPermissionProvider : IPermissionProvider
	{
		private readonly GetPermission<TPermissionProvider> _delegate;

		public CurrentUserPermissionGenericAdminSimpleMasterDataSecurityManagerFactory(
			GetPermission<TPermissionProvider> @delegate)
		{
			this._delegate = @delegate
				?? throw new ArgumentNullException(nameof(@delegate));
		}

		public IGenericAdminSimpleMasterDataSecurityManager CreateManager(IServiceProvider serviceProvider)
		{
			return new CurrentUserPermissionGenericAdminSimpleMasterDataSecurityManager<TPermissionProvider>(
				this._delegate, serviceProvider.GetRequiredService<IPermissionProviderAccessor<TPermissionProvider>>(),
				serviceProvider.GetRequiredService<CurrentUserPermissionService>());
		}
	}
}

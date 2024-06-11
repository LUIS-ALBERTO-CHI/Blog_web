using FwaEu.Fwamework.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public class PermissionUserTaskAccessManager<TTask, TPermissionProvider> : IUserTaskAccessManager<TTask>
		where TTask : class, IUserTask
		where TPermissionProvider : IPermissionProvider
	{
		private readonly Func<TPermissionProvider, IPermission> _getPermission;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public PermissionUserTaskAccessManager(
			Func<TPermissionProvider, IPermission> getPermission,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._getPermission = getPermission
				?? throw new ArgumentNullException(nameof(getPermission));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		public async Task<bool> IsAccessibleAsync()
		{
			return await this._currentUserPermissionService.HasPermissionAsync(this._getPermission);
		}
	}

	public static class PermissionUserTaskAccessManagerBuilderExtensions
	{
		public static UserTaskServicesBuilder AddPermissionAccessManager<TPermissionProvider>(
			this UserTaskServicesBuilder builder,
			Func<TPermissionProvider, IPermission> getPermission)

			where TPermissionProvider : IPermissionProvider
		{
			var userTaskType = builder.UserTaskType;
			var accessManagerType = typeof(IUserTaskAccessManager<>).MakeGenericType(userTaskType);

			builder.ServiceCollection.AddScoped(accessManagerType,
				sp =>
				{
					var permissionAccessManagerType = typeof(PermissionUserTaskAccessManager<,>)
						.MakeGenericType(userTaskType, typeof(TPermissionProvider));

					return Activator.CreateInstance(permissionAccessManagerType, getPermission,
						sp.GetRequiredService<CurrentUserPermissionService>());
				}
			);

			return builder;
		}
	}
}

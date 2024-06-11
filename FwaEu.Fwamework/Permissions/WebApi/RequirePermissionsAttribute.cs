using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Permissions.WebApi
{
	public enum MultiplePermissionOperation { All, Any }

	public class RequirePermissionsAttribute : ActionFilterAttribute
	{
		private class Permission : IPermission
		{
			public string InvariantId { get; set; }
		}

		public MultiplePermissionOperation Operation { get; }
		public string[] PermissionInvariantIds { get; }

		public RequirePermissionsAttribute(string permissionInvariantId)
			: this(MultiplePermissionOperation.All, permissionInvariantId)
		{
		}

		public RequirePermissionsAttribute(MultiplePermissionOperation operation,
			string permissionInvariantId, params string[] otherPermissionInvariantIds)
		{
			if (String.IsNullOrEmpty(permissionInvariantId))
			{
				throw new ArgumentNullException(nameof(permissionInvariantId));
			}

			this.Operation = operation;
			this.PermissionInvariantIds = new[] { permissionInvariantId }
				.Concat(otherPermissionInvariantIds).ToArray();
		}

		protected IEnumerable<IPermission> GetPermissions()
		{
			return this.PermissionInvariantIds
				.Select(pii => new Permission() { InvariantId = pii });
		}

		protected async Task<bool> IsAuthorizedAsync(ActionExecutingContext context)
		{
			var serviceProvider = context.HttpContext.RequestServices;
			var currentUserPermissionService = serviceProvider.GetRequiredService<CurrentUserPermissionService>();

			var currentUser = currentUserPermissionService.CurrentUserService.User;
			if (currentUser == null)
			{
				return false;
			}

			switch (this.Operation)
			{
				case MultiplePermissionOperation.All:
					foreach (var permission in this.GetPermissions())
					{
						var hasPermission = await currentUserPermissionService.HasPermissionAsync(permission);
						if (!hasPermission)
						{
							return false;
						}
					}
					return true;

				case MultiplePermissionOperation.Any:
					foreach (var permission in this.GetPermissions())
					{
						var hasPermission = await currentUserPermissionService.HasPermissionAsync(permission);
						if (hasPermission)
						{
							return true;
						}
					}
					return false;
			}

			throw new NotSupportedException();
		}

		public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var isAuthorized = await this.IsAuthorizedAsync(context);
			
			if (!isAuthorized)
			{
				context.Result = new ForbidResult();
			}

			await base.OnActionExecutionAsync(context, next);
		}
	}
}

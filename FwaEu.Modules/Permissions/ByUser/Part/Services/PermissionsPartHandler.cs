using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.Permissions.ByUser.Part.Services
{
	public class PermissionsPartHandler : EditablePartHandler<PermissionsPartModel, PermissionsPartModel>
	{
		public const string PartName = "Permissions";
		public override string Name => PartName;

		public PermissionsPartHandler(
			UserSessionContext userSessionContext,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public override async Task<PermissionsPartModel> LoadAsync()
		{
			var repositorysession = this._userSessionContext.SessionContext.RepositorySession;
			var user = this._userSessionContext.LoadingModel;

			var invariantIds = await repositorysession
				.Create<UserPermissionEntityRepository>()
				.QueryByUserId(user.Id)
				.Select(up => up.Permission.InvariantId)
				.ToListAsync();

			return new PermissionsPartModel()
			{
				SelectedIds = invariantIds.ToArray(),
			};
		}

		public override Task<IPartSaveResult> SaveAsync(PermissionsPartModel model)
		{
			return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
			{
				var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
				var user = this._userSessionContext.SaveUserEntity;

				var userPermissionRepository = repositorySession.Create<UserPermissionEntityRepository>();
				var permissionRepository = repositorySession.Create<PermissionEntityRepository>();

				var currentSelectedInvariantIds = await userPermissionRepository.QueryByUserId(user.Id)
					.Select(up => up.Permission.InvariantId).ToListAsync();

				var invariantIdsToDelete = currentSelectedInvariantIds.Except(model.SelectedIds).ToArray();
				var invariantIdsToCreate = model.SelectedIds.Except(currentSelectedInvariantIds).ToArray();

				if (invariantIdsToDelete.Any())
				{
					var userPermissionToDelete = userPermissionRepository.Query()
						.Where(up => invariantIdsToDelete.Contains(up.Permission.InvariantId));
					await userPermissionRepository.Query().Where(up => userPermissionToDelete.Contains(up)).DeleteAsync(CancellationToken.None);
				}

				foreach (var invariantIdToCreate in invariantIdsToCreate)
				{
					var permission = await permissionRepository.GetByInvariantIdAsync(invariantIdToCreate);

					await userPermissionRepository.SaveOrUpdateAsync(new UserPermissionEntity()
					{
						User = user,
						Permission = permission,
					});
				}
			}));
		}

		public override async Task<bool> CurrentUserCanEditAsync()
		{
			var currentUser = this._currentUserPermissionService.CurrentUserService.User;
			var user = this._userSessionContext.SaveUserEntity;

			return currentUser.Entity.Id != user.Id
				&& await this._currentUserPermissionService.HasPermissionAsync<UsersPermissionProvider>(
					provider => provider.CanAdministrateUsers);
		}
	}
}

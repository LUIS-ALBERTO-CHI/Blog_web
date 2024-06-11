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

namespace FwaEu.Modules.Permissions.ByRole.Part.Services
{
	public class RolePartHandler : EditablePartHandler<RolePartModel, RolePartModel>
	{
		public const string PartName = "Roles";
		public override string Name => PartName;

		public RolePartHandler(
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

		public override async Task<RolePartModel> LoadAsync()
		{
			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
			var user = this._userSessionContext.LoadingModel;

			var ids = await repositorySession
				.Create<UserRoleEntityRepository>()
				.QueryByUserId(user.Id)
				.Select(ur => ur.Role.Id)
				.ToListAsync();

			return new RolePartModel()
			{
				SelectedIds = ids.ToArray(),
			};
		}

		public override Task<IPartSaveResult> SaveAsync(RolePartModel model)
		{
			return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
			{

				var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
				var user = this._userSessionContext.SaveUserEntity;

				var userRoleRepository = repositorySession.Create<UserRoleEntityRepository>();
				var roleRepository = repositorySession.Create<RoleEntityRepository>();

				var currentSelectedIds = await userRoleRepository.QueryByUserId(user.Id)
					.Select(up => up.Role.Id).ToListAsync();

				var idsToDelete = currentSelectedIds.Except(model.SelectedIds).ToArray();
				var idsToCreate = model.SelectedIds.Except(currentSelectedIds).ToArray();

				if (idsToDelete.Any())
				{
					await userRoleRepository.Query()
						.Where(ur => idsToDelete.Contains(ur.Role.Id))
						.DeleteAsync(CancellationToken.None);
				}

				foreach (var idToCreate in idsToCreate)
				{
					var role = await roleRepository.GetAsync(idToCreate);

					await userRoleRepository.SaveOrUpdateAsync(new UserRoleEntity()
					{
						User = user,
						Role = role,
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

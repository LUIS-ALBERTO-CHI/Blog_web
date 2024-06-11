using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.Credentials.Parts
{
	public class CredentialsPartHandler : EditablePartHandler<object, CredentialsPartSaveModel>
	{
		public const string PartName = "Credentials";
		public override string Name => PartName;
		public override bool IsRequiredOnCreation => true;

		public CredentialsPartHandler(
			UserSessionContext userSessionContext,
			IChangePasswordCredentialsService changePasswordCredentialsService,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._changePasswordCredentialsService = changePasswordCredentialsService
				?? throw new ArgumentNullException(nameof(changePasswordCredentialsService));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly IChangePasswordCredentialsService _changePasswordCredentialsService;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public override Task<object> LoadAsync()
		{
			//NOTE: Model type is object because we dont provide properties for Get/GetAll currently
			return Task.FromResult(default(object));
		}

		public override Task<IPartSaveResult> SaveAsync(CredentialsPartSaveModel model)
		{
			return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
			{
				await this._changePasswordCredentialsService.ChangePasswordAsync(
					this._userSessionContext.SaveUserEntity.Identity, model.NewPassword, model.CurrentPassword);
			}));
		}

		public override async Task<bool> CurrentUserCanEditAsync()
		{
			var currentUser = this._currentUserPermissionService.CurrentUserService.User;
			var user = this._userSessionContext.SaveUserEntity;

			return currentUser.Entity.Id == user.Id
				|| await this._currentUserPermissionService.HasPermissionAsync<UsersPermissionProvider>(
					provider => provider.CanAdministrateUsers);
		}
	}
}

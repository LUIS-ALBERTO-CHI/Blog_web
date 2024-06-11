using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.AdminState.Parts
{
	public class AdminStatePartHandler : EditablePartHandler<AdminStatePartModel, AdminStatePartModel>
	{
		public const string PartName = "AdminState";
		public override string Name => PartName;

		public AdminStatePartHandler(
			UserSessionContext userSessionContext,
			IAuthenticationChangeInfoService authenticationChangeInfoService,
			ICurrentUserService currentUserService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._currentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));

			this._authenticationChangeInfoService = authenticationChangeInfoService
				?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly ICurrentUserService _currentUserService;
		private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;

		public override Task<AdminStatePartModel> LoadAsync()
		{
			var loadingModel = (IAdminStatePartLoadingModelPropertiesAccessor)this._userSessionContext.LoadingModel;

			return Task.FromResult(new AdminStatePartModel()
			{
				IsAdmin = loadingModel.IsAdmin,
				State = loadingModel.State
			});
		}

		public override Task<IPartSaveResult> SaveAsync(AdminStatePartModel model)
		{
			var user = this._userSessionContext.SaveUserEntity;

			if (!_currentUserService.User.Entity.IsAdmin && (user.IsAdmin || model.IsAdmin.HasValue))
			{
				throw new NonAdministratorCreateOrEditAdminUserException();
			}
			if (model.IsAdmin.HasValue)
			{
				user.IsAdmin = model.IsAdmin.Value;
			}
			user.State = model.State.Value;

			var stateChanged = user.State != model.State;
			if (stateChanged)
			{
				return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
				{
					await this._authenticationChangeInfoService.SetLastChangeDateAsync(user.Id);
				}));
			}

			return Task.FromResult<IPartSaveResult>(null);
		}

		public override Task<bool> CurrentUserCanEditAsync()
		{
			var currentUser = this._currentUserService.User;
			var user = this._userSessionContext.SaveUserEntity;

			if (user.IsAdmin && !this._currentUserService.User.Entity.IsAdmin)
			{
				throw new UserSaveValidationException("adminState", "OnlyAdminCanDefineAdminProperty",
					"Only an administrator can define that another user is administrator.");
			}

			return Task.FromResult(currentUser.Entity.Id != user.Id);
		}
	}
}

using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Modules.UserNotifications;
using FwaEu.Modules.UserTasksUserNotifications;
using FwaEu.BlogWeb.FarmManager.UserTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.FarmerUserPart
{
	public class UserFarmerModel
	{
		public string Pseudonym { get; set; }
	}

	public class UserFarmerPartHandler : EditablePartHandler<UserFarmerModel, UserFarmerModel>
	{
		public override string Name => "Farmer";

		private readonly UserSessionContext _userSessionContext;
		private readonly CurrentUserPermissionService _currentUserPermissionService;
		private readonly IUserNotificationService _userNotificationService;
		private readonly UserTaskUserNotificationService _userTaskUserNotificationService;

		public UserFarmerPartHandler(UserSessionContext userSessionContext,
			CurrentUserPermissionService currentUserPermissionService,
			IUserNotificationService userNotificationService,
			UserTaskUserNotificationService userTaskUserNotificationService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));

			this._userNotificationService = userNotificationService
				?? throw new ArgumentNullException(nameof(userNotificationService));

			this._userTaskUserNotificationService = userTaskUserNotificationService
				?? throw new ArgumentNullException(nameof(userTaskUserNotificationService));
		}

		public override async Task<bool> CurrentUserCanEditAsync()
		{
			var currentUser = this._currentUserPermissionService.CurrentUserService.User;
			var user = this._userSessionContext.SaveUserEntity;

			return currentUser.Entity.Id == user.Id
				|| await this._currentUserPermissionService.HasPermissionAsync<UsersPermissionProvider>(
					provider => provider.CanAdministrateUsers);
		}

		public override Task<UserFarmerModel> LoadAsync()
		{
			var loadingModel = (IFarmerPartLoadingModelPropertiesAccessor)this._userSessionContext.LoadingModel;

			return Task.FromResult(new UserFarmerModel()
			{
				Pseudonym = loadingModel.FarmerPseudonym,
			});
		}

		public override Task<IPartSaveResult> SaveAsync(UserFarmerModel model)
		{
			var entity = (IFarmerPartEntityPropertiesAccessor)this._userSessionContext.SaveUserEntity;
			var pseudonymWasNotSet = String.IsNullOrEmpty(entity.FarmerPseudonym);

			entity.FarmerPseudonym = model.Pseudonym;

			if (pseudonymWasNotSet && !String.IsNullOrEmpty(model.Pseudonym))
			{
				//NOTE: The notifications will be sent afther the entity is persisted in the database (PartSaveResult)

				return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterTransactionTask: async () =>
				{
					// NOTE: We could use "this._userTaskUserNotificationService.UserNotificationService" but
					// it's to demonstrate that you can use directly send UserNotification, you don't always need UserTask

					await this._userNotificationService.SendNotificationAsync(
						"NewFarmer", // NOTE: Identifier for dispatch on client-side
						new
						{
							Id = this._userSessionContext.SaveUserEntity.Id,
							Pseudonym = model.Pseudonym
						},
						this._userSessionContext.SaveUserEntity.Identity);

					// NOTE: Calculate an user task and send it's result to the clients, which will
					// do anything they wants with (update menu active item count, show notification, refresh other values...)

					await this._userTaskUserNotificationService.ExecuteUserTaskAsync<
						FarmerCountUserTask, object, FarmCountResult>(
						null, // NOTE: Parameters of the UserTask. This task has no parameters, you can use type <object> in the generic signature.
						this._userSessionContext.SaveUserEntity.Identity, CancellationToken.None);
				}));
			}

			return Task.FromResult<IPartSaveResult>(null);
		}
	}
}

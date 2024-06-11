using FwaEu.Fwamework;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter.Parts
{
	public class UserPerimetersPartHandler : EditablePartHandler<UserPerimeterPartModel, UserPerimeterPartModel>
	{
		public UserPerimetersPartHandler(
			UserSessionContext userSessionContext,
			IUserPerimeterService userPerimeterService,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._userPerimeterService = userPerimeterService
				?? throw new ArgumentNullException(nameof(userPerimeterService));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		public const string PartName = "Perimeters";
		public override string Name => PartName;

		private readonly UserSessionContext _userSessionContext;
		private readonly IUserPerimeterService _userPerimeterService;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public override async Task<UserPerimeterPartModel> LoadAsync()
		{
			var user = this._userSessionContext.LoadingModel;
			var accesses = await this._userPerimeterService.GetAccessesAsync(user.Id);

			var entries = accesses.Select(a => new UserPerimeterEntryPartModel()
			{
				Key = a.Key,
				HasFullAccess = a.HasFullAccess,
				AccessibleIds = a.AccessibleIds == null
					? null : a.AccessibleIds.ToArray()

			}).ToArray();

			return new UserPerimeterPartModel() { Entries = entries, };
		}

		public override Task<IPartSaveResult> SaveAsync(UserPerimeterPartModel model)
		{
			return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
			{
				var user = this._userSessionContext.SaveUserEntity;

				foreach (var entry in model.Entries)
				{
					var serviceModel = new UserPerimeterModel(
						entry.Key, entry.HasFullAccess,
						entry.AccessibleIds == null ? null
							: entry.AccessibleIds.Cast<object>().ToArray());

					await this._userPerimeterService.UpdatePerimeterAsync(
						user.Id, serviceModel);
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

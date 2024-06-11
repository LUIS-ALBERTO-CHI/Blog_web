using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.CultureSettings.Parts
{
	public class CultureSettingsPartHandler : EditablePartHandler<CultureSettingsPartModel, CultureSettingsPartModel>
	{
		public const string PartName = "CultureSettings";
		public override string Name => PartName;
		public override bool IsRequiredOnCreation => true;
		public override bool IsRequiredOnUpdate => true;

		public CultureSettingsPartHandler(
			UserSessionContext userSessionContext,
			ICulturesService culturesService,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly ICulturesService _culturesService;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public override async Task<CultureSettingsPartModel> LoadAsync()
		{
			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
			var user = this._userSessionContext.LoadingModel;

			var entity = await repositorySession
				.Create<UserCultureSettingsEntityRepository>()
				.GetByUserIdAsync(user.Id);

			return entity == null
				? null
				: new CultureSettingsPartModel()
				{
					LanguageTwoLetterIsoCode = entity.LanguageTwoLetterIsoCode,
				};
		}

		public override Task<IPartSaveResult> SaveAsync(CultureSettingsPartModel model)
		{
			return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
			{
				var user = this._userSessionContext.SaveUserEntity;
				var repository = this._userSessionContext.SessionContext.RepositorySession.Create<UserCultureSettingsEntityRepository>();

				var entity = (await repository.GetByUserIdAsync(user.Id))
					?? new UserCultureSettingsEntity() { User = user };

				if (!this._culturesService.AvailableCultures
					.Select(ac => ac.TwoLetterISOLanguageName)
					.Contains(model.LanguageTwoLetterIsoCode))
				{
					throw new UserSaveValidationException("cultureSettings", "SelectedLanguageNotAvailable",
						"The selected language is not available.");
				}

				entity.LanguageTwoLetterIsoCode = model.LanguageTwoLetterIsoCode;

				await repository.SaveOrUpdateAsync(entity);
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

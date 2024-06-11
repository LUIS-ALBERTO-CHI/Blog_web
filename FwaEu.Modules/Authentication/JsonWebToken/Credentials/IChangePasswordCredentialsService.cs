using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.Credentials
{
	public interface IChangePasswordCredentialsService
	{
		Task ChangePasswordAsync(string identity, string newPassword, string currentPassword = null);
	}

	public class DefaultChangePasswordCredentialsService : IChangePasswordCredentialsService
	{
		public DefaultChangePasswordCredentialsService(
			UserSessionContext userSessionContext,
			IPasswordHasher passwordHasher,
			IAuthenticationChangeInfoService authenticationChangeInfoService,
			ICurrentUserService currentUserService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._passwordHasher = passwordHasher
				?? throw new ArgumentNullException(nameof(passwordHasher));

			this._authenticationChangeInfoService = authenticationChangeInfoService
				?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));

			this._currentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;
		private readonly ICurrentUserService _currentUserService;


		public async Task ChangePasswordAsync(string identity, string newPassword, string currentPassword = null)
		{
			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
			var user = this._userSessionContext.SaveUserEntity;

			if (user.Identity != identity)
			{
				throw new NotSupportedException(); // NOTE: Identity is not required in this implementation because we use SessionContext
			}

			var repository = repositorySession.Create<UserCredentialsEntityRepository>();
			var entity = (await repository.GetByUserIdAsync(user.Id))
				?? new UserCredentialsEntity() { User = user };
			if (currentPassword != null)
			{
				var currentPasswordHash = this._passwordHasher.Hash(currentPassword);
				if (entity.PasswordHash != currentPasswordHash)
				{
					throw new UserSaveValidationException("credentials", "PasswordEnteredMustBeSameAsCurrentPassword",
						"The current password you entered is not correct.");
				}
			}
			if (newPassword != null)
			{
				if (newPassword.Length < 4)
				{
					throw new UserSaveValidationException("credentials", "PasswordMustBeAtLeast4Chars",
						"The password must contain at least 4 chars.");
				}

				var newPasswordHash = this._passwordHasher.Hash(newPassword);

				if (entity.PasswordHash == newPasswordHash)
				{
					throw new UserSaveValidationException("credentials", "NewPasswordEnteredMustBeDifferentFromCurrentPassword",
						"The current password is the same as the new one.");
				}

				entity.PasswordHash = newPasswordHash;
				await repository.SaveOrUpdateAsync(entity);

				if (user.Id != _currentUserService.User.Entity.Id)
					await this._authenticationChangeInfoService.SetLastChangeDateAsync(user.Id);

			}
		}
	}
}

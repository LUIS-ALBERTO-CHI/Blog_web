using FwaEu.Fwamework;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using FluentNHibernate.Data;
using MimeKit;
using Microsoft.Extensions.Configuration;
using FwaEu.Modules.Users.CultureSettings;
using System.Globalization;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Mail.HtmlMailSender;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials;
using FwaEu.Fwamework.Application;

namespace FwaEu.Modules.PasswordRecovery
{
	public class DefaultUserPasswordRecoveryService : IUserPasswordRecoveryService
	{
		public DefaultUserPasswordRecoveryService(
			MainSessionContext sessionContext,
			IPasswordHasher passwordHasher,
			IAuthenticationChangeInfoService authenticationChangeInfoService,
			ICulturesService culturesService,
			IHtmlMailSender<UserPasswordRecoveryMailModel> htmlMailSender,
			IConfiguration config,
			IApplicationInfo applicationInfo
			)
		{
			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			this._passwordHasher = passwordHasher
				?? throw new ArgumentNullException(nameof(passwordHasher));

			this._authenticationChangeInfoService = authenticationChangeInfoService
				?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));

			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));

			this._htmlMailSender = htmlMailSender
				?? throw new ArgumentNullException(nameof(htmlMailSender));

			this._config = config
				?? throw new ArgumentNullException(nameof(config));

			this._applicationInfo = applicationInfo
				?? throw new ArgumentNullException(nameof(applicationInfo));
		}

		private readonly MainSessionContext _sessionContext;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;
		private readonly ICulturesService _culturesService;
		private readonly IHtmlMailSender<UserPasswordRecoveryMailModel> _htmlMailSender;
		private readonly IConfiguration _config;
		private readonly IApplicationInfo _applicationInfo;

		public async Task ReinitializePasswordAsync(string email)
		{
			var repositorySession = this._sessionContext.RepositorySession;
			var existingUser = await repositorySession.Create<IUserEntityRepository>()
				   .GetByIdentityAsync(email);

			var guid = Guid.NewGuid();

			var userPasswordRecoveryEntityRepository = repositorySession
				.Create<UserPasswordRecoveryEntityRepository>();
			
			var userPasswordRecoveryEntity = 
				(await userPasswordRecoveryEntityRepository.GetByUserIdAsync(existingUser.Id))
					?? new UserPasswordRecoveryEntity() { User = existingUser };

			userPasswordRecoveryEntity.Guid = guid;
			await userPasswordRecoveryEntityRepository.SaveOrUpdateAsync(userPasswordRecoveryEntity);

			var clientApplicationUrl = _config.GetValue<string>("Fwamework:ApplicationClientAbsoluteUrl"); //HACK: Waiting for discussion on https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/4889
			var passwordRecoveryUrl = $"{clientApplicationUrl}PasswordRecovery?userId={existingUser.Id}&guid={guid}"; //TODO https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/4906
			var model = new SendPasswordRecoveryModel()
			{
				RecipientName = existingUser.ToString(),
				RecipientAddress = email,
				Url = passwordRecoveryUrl
			};

			var entity = await repositorySession
			.Create<UserCultureSettingsEntityRepository>()
			.GetByUserIdAsync(existingUser.Id);

			await SendMailAsync(model, entity != null ? new CultureInfo(entity.LanguageTwoLetterIsoCode) : this._culturesService.DefaultCulture);
			await repositorySession.Session.FlushAsync();
		}

		public async Task UpdatePasswordAsync(RequestPasswordRecoveryModel model)
		{
			var repositorySession = this._sessionContext.RepositorySession;

			using (var transaction = repositorySession.Session.BeginTransaction())
			{
				var userPasswordRecoveryEntityRepository = this._sessionContext.RepositorySession
					.Create<UserPasswordRecoveryEntityRepository>();

				var userPasswordRecoveryEntity = await userPasswordRecoveryEntityRepository
				   .Query()
				   .FirstOrDefaultAsync(uc => uc.User.Id == model.UserId);

				if (userPasswordRecoveryEntity == null || userPasswordRecoveryEntity.Guid != model.Guid)
				{
					throw new UserPasswordRecoveryNotFoundException("User password recovery request not found.");
				}

				var existingUser = await repositorySession.GetOrNotFoundExceptionAsync<UserEntity, int, IUserEntityRepository>(model.UserId);

				var userCredentialsEntityRepository = repositorySession
					.Create<UserCredentialsEntityRepository>();

				var userCredentialsEntity = (await userCredentialsEntityRepository.GetByUserIdAsync(model.UserId))
					?? new UserCredentialsEntity() { User = existingUser };

				if (model.NewPassword != null)
				{
					var newPasswordHash = this._passwordHasher.Hash(model.NewPassword);
					if (userCredentialsEntity.PasswordHash != newPasswordHash)
					{
						userCredentialsEntity.PasswordHash = newPasswordHash;
						await userCredentialsEntityRepository.SaveOrUpdateAsync(userCredentialsEntity);
						await this._authenticationChangeInfoService.SetLastChangeDateAsync(model.UserId);
					}
				}

				userPasswordRecoveryEntity.Guid = null;
				await userPasswordRecoveryEntityRepository.SaveOrUpdateAsync(userPasswordRecoveryEntity);
				await transaction.CommitAsync();
				await repositorySession.Session.FlushAsync();
			}
		}

		private async Task SendMailAsync(SendPasswordRecoveryModel model, CultureInfo culture)
		{
			await this._htmlMailSender.SendHtmlMailAsync(new UserPasswordRecoveryMailModel
			{
				Greetings = UserPasswordRecovery.ResourceManager.GetString("Greetings", culture) + " " + model.RecipientName,
				Title = UserPasswordRecovery.ResourceManager.GetString("ToRecoverYourPasswordTitle", culture),
				Message = UserPasswordRecovery.ResourceManager.GetString("ToRecoverYourPasswordMessage", culture),
				Link = model.Url,
			}, message =>
			{
				message.Subject = String.Format(UserPasswordRecovery.ResourceManager.GetString("PasswordRecovery", culture), _applicationInfo.Name);
				message.To.Add(MailboxAddress.Parse(model.RecipientAddress));
			});
		}
	}
}


using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.UserNotifications;
using FwaEu.BlogWeb.FarmManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Services
{
	public interface IFarmResponsibleService
	{
		Task SetResponsibleAsync(int farmId, int? responsibleUserId);
	}

	public class DefaultFarmResponsibleService : IFarmResponsibleService
	{
		private readonly MainSessionContext _sessionContext;
		private readonly IUserNotificationService _userNotificationService;

		public DefaultFarmResponsibleService(
			MainSessionContext sessionContext,
			IUserNotificationService userNotificationService)
		{
			_sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			_userNotificationService = userNotificationService
				?? throw new ArgumentNullException(nameof(userNotificationService));
		}

		public async Task SetResponsibleAsync(int farmId, int? responsibleUserId)
		{
			var repositorySession = _sessionContext.RepositorySession;

			var repository = repositorySession
				.Create<FarmEntityRepository>();

			var entity = await DefaultFarmService.LoadFarmAsync(farmId, repository);
			var previousResponsibleId = entity.Responsible?.Id;

			entity.Responsible = responsibleUserId == null ? null : await repositorySession.GetOrNotFoundExceptionAsync<UserEntity, int, IUserEntityRepository>(responsibleUserId.Value);
			if (entity.Responsible != null &&
				(previousResponsibleId == null || previousResponsibleId.Value != entity.Responsible.Id))
			{
				await _userNotificationService.SendNotificationAsync(
					"FarmAffected", new { FarmId = farmId, }, entity.Responsible.Identity);
			}

			await repository.SaveOrUpdateAsync(entity);
			await repositorySession.Session.FlushAsync();
		}
	}
}

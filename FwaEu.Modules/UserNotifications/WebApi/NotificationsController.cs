using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class NotificationsController : ControllerBase
	{
		//GET /Notifications/
		[HttpGet("")]
		[ProducesResponseType(typeof(StoredNotificationApiModel[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetCurrentUserNotifications(
			[FromServices] IUserNotificationStore store,
			[FromServices] ICurrentUserService currentUserService)
		{
			var models = await store.GetNotificationsAsync(currentUserService.User.Entity.Id);

			return Ok(models
				.Select(m => new StoredNotificationApiModel(
					m.Id, m.NotificationType, m.SentOn, m.Model, m.SeenOn, m.IsSticky))
				.ToArray());
		}

		//POST /Notifications/Seen
		[HttpPost("Seen")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		public async Task<IActionResult> MarkAsSeen(
			[FromBody][Required] DateTime? date,
			[FromServices] IUserNotificationStore store,
			[FromServices] ICurrentUserService currentUserService)
		{
			await store.MarkAsSeenAsync(
				currentUserService.User.Entity.Id, date.Value);

			return Ok();
		}

		//DELETE /Notifications/{id}
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteForCurrentUser(
			[FromRoute][Required] Guid id,
			[FromServices] IUserNotificationStore store,
			[FromServices] ICurrentUserService currentUserService)
		{
			await store.DeleteAsync(
				currentUserService.User.Entity.Id, id);

			return Ok();
		}
	}
}

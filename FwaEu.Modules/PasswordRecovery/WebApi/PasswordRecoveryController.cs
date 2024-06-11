
using System;
using System.Threading.Tasks;
using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FwaEu.Modules.PasswordRecovery.WebApi
{
	[ApiController]
	[Route("[controller]")]
	public class PasswordRecoveryController : ControllerBase
	{
		[AllowAnonymous]
		[HttpPost(nameof(ReinitializePassword))]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult> ReinitializePassword(
			[FromBody]UserPasswordRecoveryModel model,
			[FromServices]IUserPasswordRecoveryService service)
		{
			try
			{
				await service.ReinitializePasswordAsync(model.Email);
			}
			catch (UserNotFoundException)
			{
				return NotFound($"User not found with email '{model.Email}'.");
			}
			return Ok();
		}

		[AllowAnonymous]
		[HttpPost(nameof(UpdatePassword))]
		public async Task UpdatePassword(
			[FromBody]RequestPasswordRecoveryModel model,
			[FromServices]IUserPasswordRecoveryService service)
		{
			var serviceModel = new PasswordRecovery.RequestPasswordRecoveryModel
				(userId: model.UserId.Value,
				guid: model.Guid.Value,
				newPassword: model.NewPassword);
			await service.UpdatePasswordAsync(serviceModel);
		}
	}
}

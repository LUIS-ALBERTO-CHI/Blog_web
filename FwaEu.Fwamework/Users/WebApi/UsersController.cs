using FwaEu.Fwamework.Permissions.WebApi;
using FwaEu.Fwamework.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		[HttpGet("getAll")]
		[ProducesResponseType(typeof(UserGetAllResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[RequirePermissions(nameof(UsersPermissionProvider.CanAdministrateUsers))]
		public async Task<ActionResult<UserGetAllResponseModel>> GetAll([FromServices] IUserService userService)
		{
			var users = await userService.GetAllForAdminAsync();

			return Ok(
				users.Select(u => new UserGetAllResponseModel
				{
					Id = u.Id,
					Parts = u.Parts,
				}));
		}

		private async Task<ActionResult<UserGetResponseModel>> GetByIdAsync(int id, IUserService userService)
		{
			try
			{
				var user = await userService.GetForAdminAsync(id);

				return Ok(new UserGetResponseModel
				{
					Id = user.Id,
					Parts = user.Parts,
				});
			}
			catch (UserNotFoundException)
			{
				return NotFound(NotFoundWithUserId(id));
			}
		}

		[HttpGet(nameof(Current))]
		[ProducesResponseType(typeof(UserGetResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<UserGetResponseModel>> Current(
			[FromServices] ICurrentUserService currentUserService,
			[FromServices] IUserService userService)
		{
			var userId = currentUserService.User.Entity.Id;
			return await this.GetByIdAsync(userId, userService);
		}

		[HttpGet("get/{id}")]
		[ProducesResponseType(typeof(UserGetResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<UserGetResponseModel>> Get(int id,
			[FromServices] IUserService userService)
		{
			return await this.GetByIdAsync(id, userService);
		}

		private static string CamelToPascal(string value)
		{
			return value.Substring(0, 1).ToUpper() + value.Substring(1);
		}

		private static string PascalToCamel(string propertyName)
		{
			return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
		}

		[HttpPut("save/{id}")]
		[ProducesResponseType(typeof(UserSaveResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<UserSaveResponseModel>> Save(
			[FromRoute] int id,
			[FromBody] UserSaveModel model,
			[FromServices] IUserService userService,
			[FromServices] IModelValidationService validationService,
			[FromServices] ICurrentUserService currentUserService)
		{
			var serviceModelParts = new Dictionary<string, object>();
			var saveModelHandlerByPartName = userService.GetSaveHandlerByPartName();

			var isCurrentUserUpdatingHimself = currentUserService.User.Entity.Id == id; // NOTE: When creating, id == 0

			foreach (var handler in saveModelHandlerByPartName.Values
					.Where(h => isCurrentUserUpdatingHimself
						? h.IsRequiredOnUpdateForCurrentUser
						: id == 0
							? h.IsRequiredOnCreation
							: h.IsRequiredOnUpdate))
			{
				var camelHandlerName = PascalToCamel(handler.Name);
				if (model.Parts == null || model.Parts[camelHandlerName] == null)
				{
					return BadRequest($"A required part is missing: '{camelHandlerName}'.");
				}
			}

			if (model.Parts != null)
			{
				var errors = new Dictionary<string, string[]>();

				foreach (var part in model.Parts)
				{
					var partName = CamelToPascal(part.Key);
					if (!saveModelHandlerByPartName.ContainsKey(partName))
					{
						return BadRequest($"Unknown part: '{partName}'");
					}

					var modelType = saveModelHandlerByPartName[partName].SaveModelType
						?? typeof(Dictionary<string, object>); //NOTE: The CamelToPascal is not recursive here. If one day the model as Dictionary<> is used, we will have to apply the same behaviour as in GenericAdmin to convert all keys to PascalCasing

					var partSaveModel = part.Value.ToObject(modelType);
					var validationErrors = validationService.Validate(partSaveModel);
					if (validationErrors.Length != 0)
					{
						errors.Add(part.Key, validationErrors);
					}

					serviceModelParts.Add(partName, partSaveModel);
				}

				if (errors.Count > 0)
				{
					return BadRequest(errors);
				}
			}

			var serviceModel = new FwaEu.Fwamework.Users.UserSaveModel(serviceModelParts)
			{
				Id = id,
			};

			int userId;
			try
			{
				userId = await userService.SaveAsync(serviceModel);
			}
			catch (UserNotFoundException)
			{
				return NotFound(NotFoundWithUserId(id));
			}
			catch (UserUpdateNotAllowedException)
			{
				return Forbid($"User not updatable with id #{id}.");
			}
			catch (UserSaveValidationException ex)
			{
				var errorType = new UserSaveValidationErrorModel(ex.Message, ex.ErrorType, ex.UserPart);
				HttpContext.Features.Set(errorType);
				return Problem(
					title: ex.UserPart,
					detail: ex.Message,
					type: ex.ErrorType,
					statusCode: StatusCodes.Status412PreconditionFailed
				);
			}

			return Ok(new UserSaveResponseModel()
			{
				UserId = userId
			});
		}

		[HttpGet("Details/{id}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<object>> GetDetails(int id,
			[FromServices] IServiceProvider serviceProvider)
		{
			var userDetailsService = serviceProvider.GetService<IUserDetailsService>();
			if (userDetailsService == null)
			{
				return StatusCode(StatusCodes.Status501NotImplemented);
			}

			var details = await userDetailsService.GetUserDetailsAsync(id);
			if (details == null)
			{
				return NotFound(NotFoundWithUserId(id));
			}

			return Ok(details);
		}

		private string NotFoundWithUserId(int id)
		{
			return "User not found with id #" + id + ".";
		}
	}
}

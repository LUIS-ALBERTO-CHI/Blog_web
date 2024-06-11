using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FwaEu.Fwamework.Setup.Authentication;
using FwaEu.Fwamework.Setup.Authentication.WebApi;
using FwaEu.Fwamework.Setup.WebApi;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace FwaEu.Fwamework.Setup.WepApi
{
	[Route("[controller]")]
	[ApiController]
	public class SetupController : ControllerBase
	{
		[HttpPut]
		[Route("Execute/{taskName}")]
		[Authorize(Policy = SetupSecurityOptions.Policy)]
		[ProducesResponseType(typeof(SetupResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<SetupResponseModel>> Execute(string taskName,
			[FromBody] JObject arguments,
			[FromServices] IEnumerable<ISetupTask> tasks,
			[FromServices] IModelValidationService validationService)
		{
			var task = tasks.FirstOrDefault(t => t.Name == taskName);
			if (task == null)
			{
				return NotFound($"Task '{taskName}' not found.");
			}

			var argumentsAsJsonString = arguments?.ToString();
			var typedArguments = Newtonsoft.Json.JsonConvert.DeserializeObject(
						 argumentsAsJsonString, task.ArgumentsType ?? typeof(Dictionary<string, object>));

			var validationErrors = validationService.Validate(typedArguments);

			if (validationErrors.Length != 0)
			{
				return BadRequest(validationErrors);
			}

			var result = await task.ExecuteAsync(typedArguments);

			return new SetupResponseModel()
			{
				Data = result.Data,
				Results = new SetupResultModel()
				{
					Contexts = result.ProcessResult.Contexts.Select(c => new SetupContextModel
					{
						Name = c.Name,
						ProcessName = c.ProcessName,
						ExtendedProperties = c.ExtendedProperties,
						Entries = c.Entries.Select(e => new SetupEntryModel
						{
							Type = e.Type,
							Content = e.Content,
							Details = e.Details,
						}).ToArray()
					}).ToArray()
				}
			};
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Authenticate")]
		[ProducesResponseType(typeof(SetupAuthenticatedModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult> Authenticate([FromBody][Required] SetupCredentialsApiModel credentials,
			[FromServices] ISetupAuthenticationService setupAuthenticationService)
		{
			var authenticationResult = await setupAuthenticationService.AuthenticateAsync(credentials.Login, credentials.Password);

			if (authenticationResult != null)
			{
				return Ok(authenticationResult);
			}

			return Unauthorized("Invalid credentials");
		}
	}
}
using FwaEu.Fwamework.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class UserTasksController : ControllerBase
	{
		[HttpPut]
		[Route("Execute/{name}")]
		[ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Execute(
			[FromRoute][Required] string name,
			[FromBody] JObject parameters, //NOTE: if you don't use parameters, null will not be supported, use empty JSON object: {} 
			[FromServices] IUserTaskFactoryProvider factoryProvider,
			[FromServices] IModelValidationService validationService)
		{
			var factory = await factoryProvider.FindFactoryAsync(name);

			if (factory == null)
			{
				return NotFound($"No user task found with name '{name}'.");
			}

			var userTask = factory.Create();

			var typedParameters = Newtonsoft.Json.JsonConvert.DeserializeObject(
						 parameters.ToString(), userTask.ParametersType
							?? typeof(Dictionary<string, object>));

			var validationErrors = validationService.Validate(typedParameters);
			if (validationErrors.Length != 0)
			{
				return BadRequest(validationErrors);
			}

			var result = await userTask.ExecuteAsync(typedParameters, CancellationToken.None);
			return Ok(result);
		}

		[HttpGet]
		[Route("")]
		[ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAccessibleTasks(
			[FromServices] IUserTaskFactoryProvider factoryProvider)
		{
			var accessibleTaskNames = await factoryProvider.GetAccessibleTaskNamesAsync();
			return Ok(accessibleTaskNames);
		}
	}
}
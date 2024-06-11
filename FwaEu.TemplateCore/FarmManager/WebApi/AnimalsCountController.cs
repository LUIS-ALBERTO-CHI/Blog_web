using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Permissions.WebApi;
using FwaEu.TemplateCore.FarmManager.Models;
using FwaEu.TemplateCore.FarmManager.Services;
using FwaEu.TemplateCore.FarmManager.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.WebApi
{
	[Authorize]
	[ApiController]
	[Route("/Farms/{id}/[controller]")]
	[RequirePermissions(nameof(FarmManagerPermissionProvider.CanAccessToFarmManager))]
	public class AnimalsCountController : ControllerBase
	{
		//GET /Farms/{id}/AnimalsCount
		[HttpGet("")]
		[ProducesResponseType(typeof(FarmGetAllResponseApiModel), StatusCodes.Status200OK)]
		public async Task<ActionResult<AnimalCountBySpeciesApiModel[]>> GetAllByFarmId(int id,
		[FromServices] IAnimalCountService animalCountService)
		{
			var models = await animalCountService.GetAllAsync(id);

			return Ok(models.Select(m =>
				new AnimalCountBySpeciesApiModel(
					m.Quantity, m.AnimalSpeciesId,
					m.UpdatedOn, m.UpdatedById))
				.ToArray());
		}

		private NotFoundObjectResult FarmNotFoundWithId(int id)
		{
			return NotFound($"Farm not found with id #{id}.");
		}

		//PUT /Farms/{id}/AnimalsCount
		[HttpPut("")]
		[ProducesResponseType(typeof(FarmGetAllResponseApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(FarmGetAllResponseApiModel), StatusCodes.Status404NotFound)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanSaveFarms))]
		public async Task<ActionResult<AnimalCountBySpeciesApiModel[]>> SaveOrDeleteByFarmId(int id,
			[FromBody] AnimalCountBySpeciesSaveApiModel[] models,
			[FromServices] IAnimalCountService animalCountService)
		{
			var serviceModels = models.Select(m =>
				new AnimalCountSaveModel(m.Quantity.Value, m.AnimalSpeciesId.Value))
				.ToArray();

			try
			{
				await animalCountService.SaveOrDeleteAsync(id, serviceModels);
			}
			catch (NotFoundException)
			{
				return FarmNotFoundWithId(id);
			}

			return Ok();
		}
	}
}

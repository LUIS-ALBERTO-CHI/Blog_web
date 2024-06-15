using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Permissions.WebApi;
using FwaEu.Modules.Data.Database;
using FwaEu.BlogWeb.FarmManager.Models;
using FwaEu.BlogWeb.FarmManager.Services;
using FwaEu.BlogWeb.FarmManager.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[RequirePermissions(nameof(FarmManagerPermissionProvider.CanAccessToFarmManager))]
	public class FarmsController : ControllerBase
	{
		//GET /Farms/
		[HttpGet("")]
		[ProducesResponseType(typeof(FarmGetAllResponseApiModel[]), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll(
			bool onlyFarmsWithoutAnimals,
			[FromServices] IFarmService farmService)
		{
			var farms = await farmService.GetAllAsync(onlyFarmsWithoutAnimals);

			return Ok(
				farms.Select(x => new FarmGetAllResponseApiModel
				{
					Id = x.Id,
					Name = x.Name,
					PostalCodeId = x.PostalCodeId,
					RegionId = x.RegionId,
					CategorySize = x.CategorySize,
					MainActivityId = x.MainActivityId,
					SellingPriceInEurosWithoutTaxes = x.SellingPriceInEurosWithoutTaxes,
					RecruitEmployees = x.RecruitEmployees,
					OpeningDate = x.OpeningDate,
					ClosingDate = x.ClosingDate,
					AnimalCount = x.AnimalCount,
					UpdatedOn = x.UpdatedOn,
					UpdatedById = x.UpdatedById
				}).ToArray());
		}

		private NotFoundObjectResult NotFoundWithId(int id)
		{
			return NotFound($"Farm not found with id #{id}.");
		}

		// GET /Farms/{id}/GeneralInformation
		[HttpGet("{id}/GeneralInformation")]
		[ProducesResponseType(typeof(FarmGetAllResponseApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetGeneralInformation(
			[FromRoute] int id,
			[FromServices] IFarmService farmService)
		{
			var farm = await farmService.GetGeneralInformationAsync(id);

			if (farm == null)
			{
				return NotFoundWithId(id);
			}

			return Ok(new FarmGetGeneralInformationApiModel()
			{
				Id = farm.Id,
				Name = farm.Name,
				PostalCodeId = farm.PostalCodeId,
				CategorySize = farm.CategorySize,
				MainActivityId = farm.MainActivityId,
				SellingPriceInEurosWithoutTaxes = farm.SellingPriceInEurosWithoutTaxes,
				RecruitEmployees = farm.RecruitEmployees,
				OpeningDate = farm.OpeningDate,
				ClosingDate = farm.ClosingDate,
				Comments = farm.Comments,
				CreatedOn = farm.CreatedOn,
				CreatedById = farm.CreatedById,
				UpdatedOn = farm.UpdatedOn,
				UpdatedById = farm.UpdatedById,
			});
		}

		private static FarmGeneralInformationModel CreateFarmGeneralInformationModel(FarmSaveGeneralInformationApiModel model)
		{
			return new FarmGeneralInformationModel()
			{
				Name = model.Name,
				PostalCodeId = model.PostalCodeId.Value,
				CategorySize = model.CategorySize,
				MainActivityId = model.MainActivityId.Value,
				SellingPriceInEurosWithoutTaxes = model.SellingPriceInEurosWithoutTaxes,
				RecruitEmployees = model.RecruitEmployees.Value,
				OpeningDate = model.OpeningDate.Value,
				ClosingDate = model.ClosingDate,
				Comments = model.Comments,
			};
		}

		// POST /Farms/
		[HttpPost("")]
		[ProducesResponseType(typeof(FarmSaveResultApiModel), StatusCodes.Status200OK)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanSaveFarms))]
		public async Task<IActionResult> Create(
			[FromBody] FarmSaveGeneralInformationApiModel model,
			[FromServices] IFarmService farmService)
		{
			try
			{
				var serviceModel = CreateFarmGeneralInformationModel(model);
				var farmId = await farmService.SaveAsync(serviceModel);
				return Ok(new FarmSaveResultApiModel(farmId));
			}
			catch (DatabaseException e)
			{
				return Conflict(new
				{
					Type = e.Type,
					Message = e.Message
				});
			}
		}

		// PUT /Farms/{id}/GeneralInformation
		[HttpPut("{id}/GeneralInformation")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanSaveFarms))]
		public async Task<IActionResult> UpdateGeneralInformation(
			[FromRoute] int id,
			[FromBody] FarmSaveGeneralInformationApiModel model,
			[FromServices] IFarmService farmService)
		{
			var serviceModel = CreateFarmGeneralInformationModel(model);
			serviceModel.Id = id;

			try
			{
				await farmService.SaveAsync(serviceModel);
			}
			catch (NotFoundException)
			{
				return NotFoundWithId(serviceModel.Id);
			}
			catch (DatabaseException e)
			{
				return Conflict(new
				{
					Type = e.Type,
					Message = e.Message
				});
			}

			return Ok();
		}

		// PUT /Farms/{id}/Responsible
		[HttpPut("{id}/Responsible")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status412PreconditionFailed)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanSaveFarms))]
		public async Task<IActionResult> SetResponsible(
			[FromRoute] int id,
			[FromBody] SetResponsibleApiModel model,
			[FromServices] IFarmResponsibleService farmResponsibleService)
		{
			try
			{
				await farmResponsibleService.SetResponsibleAsync(id, model.ResponsibleId);
			}
			catch (NotFoundException)
			{
				return NotFoundWithId(id);
			}
			catch (ReferenceNotFoundException)
			{
				return StatusCode(StatusCodes.Status412PreconditionFailed);
			}

			return Ok();
		}

		// DELETE /Farms/{id}
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanDeleteFarms))]
		public async Task<IActionResult> Delete(
			[FromRoute] int id,
			[FromServices] IFarmService farmService)
		{
			try
			{
				await farmService.DeleteAsync(id);
			}
			catch (NotFoundException)
			{
				return NotFoundWithId(id);
			}

			return Ok();
		}

		[HttpPost("{id}/TestLog/{farmName}")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		public async Task<IActionResult> TestLog(
			[FromRoute] int id,
			[FromRoute] string farmName,
			[FromServices] ILogger<FarmsController> logger)
		{
			logger.LogError(new Exception("Exception message."),
				$"Test log with id #{id}.");

			logger.LogError(new Exception("Exception message."),
				"Test log with id #{Id}.", id);

			var complexObject = new
			{
				Id = id,
				Name = "Romain",
				Age = 6,
				Car = new
				{
					Brand = "Audi",
					Name = "A3 1.9 TDi"
				}
			};

			logger.LogError(new Exception("Exception message."),
				"Test log with complex object: {@ComplexObject}.", complexObject);

			return Ok(farmName);
		}
	}
}

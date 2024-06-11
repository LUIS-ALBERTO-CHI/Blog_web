using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.WebApi;
using FwaEu.Fwamework.Permissions.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.Photo.WebApi
{
	[Authorize]
	[ApiController]
	[RequirePermissions(nameof(FarmManagerPermissionProvider.CanAccessToFarmManager))]
	public class FarmsPhotosController : ControllerBase
	{
		private NotFoundObjectResult FarmNotFound(int farmId)
		{
			return NotFound(new NotFoundApiModel(NotFoundType.Farm, $"No farm found with id #{farmId}."));
		}

		/// <summary>
		/// POST /Farms/{id}/Photo
		/// </summary>
		[HttpPost("Farms/{farmId}/Photo")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NotFoundApiModel), StatusCodes.Status404NotFound)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanSaveFarms))]
		public async Task<IActionResult> Save(
			[FromRoute] int farmId,
			[FromForm][Required] IFormFile file,
			[FromServices] IFarmPhotoService farmPhotoService)
		{
			var photo = new FormFileFileAdapter(file);

			try
			{
				await farmPhotoService.SavePhotoAsync(farmId, photo);
			}
			catch (NotFoundException)
			{
				return FarmNotFound(farmId);
			}

			return Ok();
		}

		/// <summary>
		/// GET /Farms/{id}/Photo
		/// </summary>
		[HttpGet("Farms/{farmId}/Photo")]
		[ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NotFoundApiModel), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get(
			[FromRoute] int farmId,
			[FromServices] IFarmPhotoService farmPhotoService,
			[FromServices] IContentTypeProvider contentTypeProvider)
		{
			var photo = default(InMemoryFile);

			try
			{
				photo = await farmPhotoService.GetPhotoAsync(farmId);
			}
			catch (NotFoundException)
			{
				return FarmNotFound(farmId);
			}

			if (photo == null)
			{
				return NotFound(new NotFoundApiModel(NotFoundType.Photo, $"No photo found for farm with id #{farmId}."));
			}

			if (!contentTypeProvider.TryGetContentType(photo.Name, out string contentType))
			{
				contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
			}

			return File(photo.MemoryStream.ToArray(), contentType, photo.Name);
		}

		/// <summary>
		/// DELETE /Farms/{id}/Photo
		/// </summary>
		[HttpDelete("Farms/{farmId}/Photo")]
		[ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NotFoundApiModel), StatusCodes.Status404NotFound)]
		[RequirePermissions(nameof(FarmManagerPermissionProvider.CanDeleteFarms))]
		public async Task<IActionResult> Delete(
			[FromRoute] int farmId,
			[FromServices] IFarmPhotoService farmPhotoService)
		{
			try
			{
				var photoExistedBeforeDelete = await farmPhotoService.DeletePhotoAsync(farmId);

				if (!photoExistedBeforeDelete)
				{
					return NotFound(new NotFoundApiModel(NotFoundType.Photo, $"No photo found for farm with id #{farmId}."));
				}
			}
			catch (NotFoundException)
			{
				return FarmNotFound(farmId);
			}

			return Ok();
		}
	}
}

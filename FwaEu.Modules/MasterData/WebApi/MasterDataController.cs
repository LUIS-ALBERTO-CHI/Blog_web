using FwaEu.Fwamework.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class MasterDataController : ControllerBase
	{
		[HttpPost(nameof(GetChangeInfos))]
		[ProducesResponseType(typeof(MasterDataGetChangeInfosResponseModel), StatusCodes.Status200OK)]
		public async Task<ActionResult<object>> GetChangeInfos(
			[FromBody] GetChangeInfosParametersModel[] parameters,
			[FromServices] IMasterDataService masterDataService)
		{

			var changesInfos = await masterDataService.GetChangesInfosAsync(
				parameters.Select(p => new RelatedParameters<MasterDataProviderGetChangesParameters>(
						p.MasterDataKey, new MasterDataProviderGetChangesParameters()))
					.ToArray());

			return Ok(changesInfos.Select(ci => new MasterDataGetChangeInfosResponseModel
			{
				MasterDataKey = ci.Key,
				Count = ci.ChangesInfo.Count,
				MaximumUpdatedOn = ci.ChangesInfo.MaximumUpdatedOn,
			}));
		}

		[HttpPost(nameof(GetModels))]
		[ProducesResponseType(typeof(MasterDataGetModelsResponseModel), StatusCodes.Status200OK)]
		public async Task<ActionResult<object>> GetModels(
			[FromBody] GetModelsParametersModel[] parameters,
			[FromServices] IMasterDataService masterDataService,
			[FromServices] IUserContextLanguage userContextLanguage)
		{

			var models = await masterDataService.GetModelsAsync(
				parameters.Select(p => new RelatedParameters<MasterDataProviderGetModelsParameters>(
						p.MasterDataKey, new MasterDataProviderGetModelsParameters(
							p.Pagination == null ? null
								: new MasterDataPaginationParameters(p.Pagination.Skip.Value, p.Pagination.Take.Value),
							p.Search,
							p.OrderBy == null ? null
								: p.OrderBy.Select(ob => new OrderByParameter(ob.PropertyName, ob.Ascending.Value))
									.ToArray(),
							userContextLanguage.GetCulture())))
					.ToArray());

			return Ok(models.Select(m => new MasterDataGetModelsResponseModel
			{
				MasterDataKey = m.Key,
				Values = m.Values.Cast<object>().ToArray()
			}));
		}

		private static object[] ConvertIds(JArray ids, Type type)
		{
			return ids.Select(id => id.ToObject(type)).ToArray();
		}

		[HttpPost(nameof(GetModelsByIds))]
		[ProducesResponseType(typeof(MasterDataGetModelsResponseModel), StatusCodes.Status200OK)]
		public async Task<ActionResult<object>> GetModelsByIds(
			[FromBody] GetModelsByIdsParametersModel[] parameters,
			[FromServices] IMasterDataService masterDataService,
			[FromServices] IUserContextLanguage userContextLanguage)
		{
			var models = await masterDataService.GetModelsByIdsAsync(
				parameters.Select(p => new RelatedParameters<MasterDataProviderGetModelsByIdsParameters>(
						p.MasterDataKey, new MasterDataProviderGetModelsByIdsParameters(
							ConvertIds(p.Ids, masterDataService.GetIdType(p.MasterDataKey)),
							userContextLanguage.GetCulture())))
					.ToArray());

			return Ok(models.Select(m => new MasterDataGetModelsResponseModel
			{
				MasterDataKey = m.Key,
				Values = m.Values.Cast<object>().ToArray()
			}));
		}
	}
}

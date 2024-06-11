using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SearchEngine.WebApi
{
	[Authorize]
	[ApiController]
	[Route("Search")]
	public class SearchEngineController : ControllerBase
	{
		[HttpPost("{search}")]
		[ProducesResponseType(typeof(SearchResultApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string[]), StatusCodes.Status412PreconditionFailed)]
		public async Task<IActionResult> Search(
			[FromRoute] string search,
			[FromBody] SearchParametersApiModel[] parameters,
			[FromServices] ISearchEngineService searchEngineService)
		{
			var serviceParameters = parameters
				.Select(p => new SearchParameters(p.Key,
					new SearchPagination(p.Skip.Value, p.Take.Value)))
				.ToArray();


			try
			{
				var results = await searchEngineService.SearchAsync(search, serviceParameters);

				return Ok(results
					.Select(r => new SearchResultApiModel(r.Key, r.Results.ToArray()))
					.ToArray());
			}
			catch (ProvidersNotFoundException ex)
			{
				return StatusCode(StatusCodes.Status412PreconditionFailed, ex.ProviderKeys);
			}
		}
	}
}

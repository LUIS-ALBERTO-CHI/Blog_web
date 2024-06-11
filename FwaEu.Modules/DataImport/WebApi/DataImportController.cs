using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.DataImport.WebApi
{
	[ApiController]
	[Route("[controller]")]
	public class DataImportController : ControllerBase
	{
		[HttpPost(nameof(Import))]
		[ProducesResponseType(typeof(DataImportResultModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(NoImporterFoundModel), StatusCodes.Status415UnsupportedMediaType)]
		public async Task<IActionResult> Import([FromServices]IDataImportService importService)
		{
			var requestFiles = HttpContext.Request.Form.Files;
			var dataImportFiles = requestFiles.Select(file => new FormFileDataImportFileAdapter(file));
			try
			{
				var result = await importService.ImportAsync(dataImportFiles.ToArray());
				return Ok(new DataImportResponseModel()
				{
					Results = new DataImportResultModel()
					{
						Contexts = result.Contexts.Select(c => new DataImportContextModel
						{
							Name = c.Name,
							ProcessName = c.ProcessName,
							ExtendedProperties = c.ExtendedProperties,
							Entries = c.Entries.Select(e => new DataImportEntryModel
							{
								Type = e.Type,
								Content = e.Content,
								Details = e.Details,
							}).ToArray()
						}).ToArray()
					}
				});
			}
			catch (NoImporterFoundException ex)
			{
				return StatusCode(StatusCodes.Status415UnsupportedMediaType, new NoImporterFoundModel()
				{
					FileNames = ex.FileNames.ToArray()
				});
			}
		}
	}
}
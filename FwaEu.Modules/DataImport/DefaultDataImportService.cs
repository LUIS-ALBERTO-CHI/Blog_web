using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.DataImport
{
	public class DefaultDataImportService : IDataImportService
	{
		private readonly IImportService _importService;
		private readonly IImportProcessResultFactory _importProcessResultFactory;

		public DefaultDataImportService(IImportService importService, IImportProcessResultFactory importProcessResultFactory)
		{
			this._importService = importService
				?? throw new ArgumentNullException(nameof(importService));

			this._importProcessResultFactory = importProcessResultFactory
				?? throw new ArgumentNullException(nameof(importProcessResultFactory));
		}

		public async Task<ProcessResult> ImportAsync(IDataImportFile[] dataImportFiles)
		{
			var result = this._importProcessResultFactory.CreateProcessResult();
			var files = dataImportFiles.Select(file => new DataImportFileImportFileAdapter(file));

			await using (var importContext = this.CreateImportContext(result, files.ToArray()))
			{
				try
				{
					await this._importService.ImportAllFilesAsync(importContext);
				}
				catch (ProcessStoppedAfterTooManyErrorsException ex)
				{
					StopProcessAfterTooManyErrorsProcessResultListener.RemoveListener(result, ex);
				}
			}

			return result;
		}

		protected virtual ImportContext CreateImportContext(ProcessResult result, IImportFile[] files)
		{
			return new ImportContext(result, files);
		}

		
	}
}
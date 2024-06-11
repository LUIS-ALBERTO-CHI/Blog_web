using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;

namespace FwaEu.Fwamework.Setup.FileImport
{
	public abstract class FilesImportSubTask : ISubTask
	{
		protected FilesImportSubTask(IImportService importService)
		{
			this._importService = importService ?? throw new ArgumentNullException(nameof(importService));
		}

		private readonly IImportService _importService;

		protected abstract IEnumerable<IImportFile> GetFiles();

		protected virtual ImportContext CreateImportContext(ProcessResult result, IImportFile[] files)
		{
			return new ImportContext(result, files);
		}

		public async Task ExecuteAsync(ProcessResult result)
		{
			await using (var importContext = this.CreateImportContext(result, this.GetFiles().ToArray()))
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
		}
	}
}

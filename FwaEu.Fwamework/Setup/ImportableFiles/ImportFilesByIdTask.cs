using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup.FileImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.ImportableFiles
{
	public class ImportFilesByIdTask : SetupTask<ImportFilesByIdTaskParameters>
	{
		public override string Name => "ImportFilesById";

		public ImportFilesByIdTask(IFileListService fileListService,
			IImportService importService, IImportProcessResultFactory importProcessResultFactory)
		{
			this._fileListService = fileListService
				?? throw new ArgumentNullException(nameof(fileListService));

			this._importService = importService
				?? throw new ArgumentNullException(nameof(importService));

			this._importProcessResultFactory = importProcessResultFactory
				?? throw new ArgumentNullException(nameof(importProcessResultFactory));
		}

		private readonly IFileListService _fileListService;
		private readonly IImportService _importService;
		private readonly IImportProcessResultFactory _importProcessResultFactory;

		public override async Task<NoDataSetupTaskResult> ExecuteAsync(ImportFilesByIdTaskParameters arguments)
		{
			var files = this._fileListService.GetFiles()
				.Where(fsm => arguments.FileIds.Contains(fsm.Id))
				.Select(fsm => new FileInfo(fsm.AbsolutePath))
				.ToArray();

			if (files.Length != arguments.FileIds.Length)
			{
				throw new InvalidOperationException("At least one of the provided file id is not matching with a server file.");
			}

			var subTask = new ImportTask(files, this._importService);
			var processResult = this._importProcessResultFactory.CreateProcessResult();

			await subTask.ExecuteAsync(processResult);

			return new NoDataSetupTaskResult(processResult);
		}

		private class ImportTask : LocalFilesImportSubTask
		{
			public ImportTask(IEnumerable<FileInfo> files, IImportService importService)
				: base(importService)
			{
				this._files = files ?? throw new ArgumentNullException(nameof(files));
			}

			private readonly IEnumerable<FileInfo> _files;

			protected override IEnumerable<FileInfo> GetFileInfos()
			{
				return this._files;
			}
		}
	}

	public class ImportFilesByIdTaskParameters
	{
		public string[] FileIds { get; set; }
	}
}

using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup.FileImport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class Base64FileImportTask : SetupTask<Base64ImportFileParameters>
	{
		public override string Name => "UploadBase64File";

		private readonly IImportService _importService;
		private readonly IImportProcessResultFactory _importProcessResultFactory;

		public Base64FileImportTask(IImportService importService, IImportProcessResultFactory importProcessResultFactory)
		{
			this._importService = importService
				?? throw new ArgumentNullException(nameof(importService));

			this._importProcessResultFactory = importProcessResultFactory
				?? throw new ArgumentNullException(nameof(importProcessResultFactory));
		}

		public async override Task<NoDataSetupTaskResult> ExecuteAsync(Base64ImportFileParameters arguments)
		{
			var adapter = new Base64FileToImportFileAdapter(_importService, arguments.Files);
			var result = this._importProcessResultFactory.CreateProcessResult();

			try
			{
				await adapter.ExecuteAsync(result);
			}
			catch (ProcessStoppedAfterTooManyErrorsException ex)
			{
				StopProcessAfterTooManyErrorsProcessResultListener.RemoveListener(result, ex);
			}

			return new NoDataSetupTaskResult(result);
		}
	}

	public class Base64ImportFileParameters
	{
		public Base64ImportFileParameters(Base64ImportFileModel[] files)
		{
			this.Files = files;
		}
		public Base64ImportFileModel[] Files { get; set; }
	}

	public class Base64ImportFileModel
	{
		public string FileName { get; set; }
		public string Content { get; set; }
	}

	public class Base64FileToImportFileAdapter : FilesImportSubTask
	{
		private Base64ImportFileModel[] _arguments { get; set; }
		public Base64FileToImportFileAdapter(IImportService service, Base64ImportFileModel[] arguments) : base(service)
		{
			this._arguments = arguments;
		}
		
		protected override IEnumerable<IImportFile> GetFiles()
		{
			return this._arguments.Select(file => new Base64ImportFile(file.FileName, file.Content));			
		}
	}
}

using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.ImportableFiles
{
	public class DownloadFilesTask : SetupTask<DownloadFilesParameters, SetupTaskResult<DownloadFilesResultModel>>
	{
		public override string Name => "DownloadFiles";

		public DownloadFilesTask(IFileListService fileListService)
		{
			this._fileListService = fileListService ?? throw new ArgumentNullException(nameof(fileListService));
		}

		private readonly IFileListService _fileListService;

		public override async Task<SetupTaskResult<DownloadFilesResultModel>> ExecuteAsync(DownloadFilesParameters arguments)
		{
			var fileContentAsBase64ByFileId = this._fileListService.GetFiles()
				.Where(fsm => arguments.FileIds.Contains(fsm.Id))
				.ToDictionary(fsm => fsm.Id,
					async fsm =>
					{
						var bytes = await File.ReadAllBytesAsync(fsm.AbsolutePath);
						return Convert.ToBase64String(bytes);
					});

			if (fileContentAsBase64ByFileId.Count != arguments.FileIds.Length)
			{
				throw new InvalidOperationException("At least one of the provided file id is not matching with a server file.");
			}

			await Task.WhenAll(fileContentAsBase64ByFileId.Values);

			return new SetupTaskResult<DownloadFilesResultModel>(new ProcessResult(),
				new DownloadFilesResultModel(
					fileContentAsBase64ByFileId
						.Select(kv => new FileContentResultModel(kv.Key, kv.Value.Result))
						.ToArray())
				);
		}
	}

	public class DownloadFilesParameters
	{
		public string[] FileIds { get; set; }
	}

	public class DownloadFilesResultModel
	{
		public DownloadFilesResultModel(FileContentResultModel[] fileContents)
		{
			this.FileContents = fileContents ?? throw new ArgumentNullException(nameof(fileContents));
		}

		public FileContentResultModel[] FileContents { get; }
	}

	public class FileContentResultModel
	{
		public FileContentResultModel(string fileId, string base64Content)
		{
			this.FileId = fileId ?? throw new ArgumentNullException(nameof(fileId)); ;
			this.Base64Content = base64Content; //NOTE: Could be null...
		}

		public string FileId { get; }
		public string Base64Content { get; }
	}
}

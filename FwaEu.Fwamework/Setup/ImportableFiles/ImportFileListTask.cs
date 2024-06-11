using FwaEu.Fwamework.Configuration;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.ImportableFiles
{
	public class ImportFileListTask : ISetupTask
	{
		public string Name => "ImportFileList";
		public Type ArgumentsType => null;

		public ImportFileListTask(IFileListService fileListService)
		{
			this._fileListService = fileListService ?? throw new ArgumentNullException(nameof(fileListService));
		}

		private readonly IFileListService _fileListService;

		public Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var fileModels = this._fileListService.GetFiles()
				.Select(fs => new FileModel
				(
					id: fs.Id,
					filePath: fs.FilePath,
					fileLengthInBytes: fs.FileLengthInBytes,
					fileLastModificationOn: fs.FileLastModificationOn,
					contentType: fs.ContentType
				))
				.ToArray();

			return Task.FromResult((ISetupTaskResult)
					new SetupTaskResult<FileListModel>(
						new ProcessResult(),
						new FileListModel(fileModels)));
		}
	}
}

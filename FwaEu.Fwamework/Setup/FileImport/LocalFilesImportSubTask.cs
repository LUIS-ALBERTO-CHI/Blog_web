using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Imports;

namespace FwaEu.Fwamework.Setup.FileImport
{
	public abstract class LocalFilesImportSubTask : FilesImportSubTask
	{
		protected LocalFilesImportSubTask(IImportService importService)
			: base(importService)
		{
		}

		protected abstract IEnumerable<FileInfo> GetFileInfos();

		protected override IEnumerable<IImportFile> GetFiles()
		{
			return this.GetFileInfos()
				.Select(fileInfo => new FileInfoImportFile(fileInfo));
		}
	}
}

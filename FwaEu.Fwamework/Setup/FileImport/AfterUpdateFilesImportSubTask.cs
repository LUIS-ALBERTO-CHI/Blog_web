using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Configuration;
using FwaEu.Fwamework.Imports;
using Microsoft.Extensions.Options;

namespace FwaEu.Fwamework.Setup.FileImport
{
	public class AfterUpdateFilesImportSubTask : LocalFilesImportSubTask, IAfterUpdateSchemaSubTask
	{
		public AfterUpdateFilesImportSubTask(IImportService importService,
			IPathFileProvider filesProvider,
			IOptions<SetupOptions> settings)
			: base(importService)
		{
			this._paths = settings.Value.AfterUpdateSchemaPaths;
			this._filesProvider = filesProvider;
		}

		private readonly IPathFileProvider _filesProvider;
		private readonly PathEntry[] _paths;

		protected override IEnumerable<FileInfo> GetFileInfos()
		{
			return this._filesProvider.GetFiles(this._paths)
				.Select(fibpe => fibpe.FileInfo);
		}
	}
}

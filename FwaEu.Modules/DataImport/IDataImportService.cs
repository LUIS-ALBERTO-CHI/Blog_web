using FwaEu.Fwamework.Data;
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
	public interface IDataImportService
	{
		public Task<ProcessResult> ImportAsync(IDataImportFile[] dataImportFiles);
	}

	public interface IDataImportFile : IFile
	{

	}
}

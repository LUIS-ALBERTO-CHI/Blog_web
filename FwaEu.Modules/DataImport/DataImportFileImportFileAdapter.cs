using FwaEu.Fwamework.Imports;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.DataImport
{
	public class DataImportFileImportFileAdapter : IImportFile
	{
		private readonly IDataImportFile _file;

		public DataImportFileImportFileAdapter(IDataImportFile file)
		{
			this._file = file ?? throw new ArgumentNullException(nameof(file));
		}

		public string Name => this._file.Name;

		public long LengthInBytes => this._file.LengthInBytes;

		public Stream OpenReadStream()
		{
			return this._file.OpenReadStream();
		}
	}
}

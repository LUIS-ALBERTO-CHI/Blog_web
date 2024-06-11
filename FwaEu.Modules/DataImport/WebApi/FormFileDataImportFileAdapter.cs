using FwaEu.Fwamework.Data.WebApi;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.DataImport.WebApi
{
	public class FormFileDataImportFileAdapter : FormFileFileAdapter, IDataImportFile
	{
		public FormFileDataImportFileAdapter(IFormFile file) : base(file)
		{
		}
}
}

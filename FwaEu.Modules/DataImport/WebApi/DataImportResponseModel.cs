using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.DataImport.WebApi
{
	public class DataImportResponseModel
	{
		public DataImportResultModel Results { get; set; }
	}

	public class DataImportResultModel
	{
		public DataImportContextModel[] Contexts { get; set; }
	}

	public class DataImportContextModel
	{
		public string Name { get; set; }
		public string ProcessName { get; set; }
		public object ExtendedProperties { get; set; }
		public DataImportEntryModel[] Entries { get; set; }
	}

	public class DataImportEntryModel
	{
		public string Type { get; set; }
		public string Content { get; set; }
		public string[] Details { get; set; }
	}

	public class NoImporterFoundModel
	{
		public string[] FileNames { get; set; }
	}
}

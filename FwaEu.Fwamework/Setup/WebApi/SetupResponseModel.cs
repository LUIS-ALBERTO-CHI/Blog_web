using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.WebApi
{
	public class SetupEntryModel
	{
		public string Type { get; set; }
		public string Content { get; set; }
		public string[] Details { get; set; }
	}

	public class SetupResponseModel
	{
		public object Data { get; set; }
		public SetupResultModel Results { get; set; }
	}

	public class SetupResultModel
	{
		public SetupContextModel[] Contexts { get; set; }
	}

	public class SetupContextModel
	{
		public string Name { get; set; }
		public string ProcessName { get; set; }
		public object ExtendedProperties { get; set; }
		public SetupEntryModel[] Entries { get; set; }
	}
}

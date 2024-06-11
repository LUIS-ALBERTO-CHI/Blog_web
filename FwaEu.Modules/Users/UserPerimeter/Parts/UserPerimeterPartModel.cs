using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter.Parts
{
	public class UserPerimeterPartModel
	{
		public UserPerimeterEntryPartModel[] Entries { get; set; }
	}

	public class UserPerimeterEntryPartModel
	{
		public string Key { get; set; }
		public bool HasFullAccess { get; set; }
		public object[] AccessibleIds { get; set; }
	}
}

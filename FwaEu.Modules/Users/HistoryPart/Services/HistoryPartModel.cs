using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Users.HistoryPart.Services
{
	public class HistoryPartModel
	{
		public int? CreatedById { get; set; }
		public DateTime CreatedOn { get; set; }
		public int? UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }
	}
}

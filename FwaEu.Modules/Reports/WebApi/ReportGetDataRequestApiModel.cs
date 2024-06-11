using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportGetDataRequestApiModel : ReportRequestApiModel
	{
		[Required]
		public JObject Filters { get; set; }
	}
}

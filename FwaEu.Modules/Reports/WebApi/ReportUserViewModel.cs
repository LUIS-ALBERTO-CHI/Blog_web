using FwaEu.Fwamework.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportUserViewModel
	{
		public int? Id { get; set; }
		[LocalizableString]
		public IDictionary Name { get; set; }
		public int UserId { get; set; }
		public int ReportId { get; set; }
		public string Value { get; set; }
	}

	public class ReportUserViewIsEnabledModel
	{
		public bool IsEnabled { get; set; }
	}
}

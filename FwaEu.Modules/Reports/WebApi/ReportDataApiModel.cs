using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class ReportDataApiModel
	{
		public ReportDataApiModel(Dictionary<string, object>[] rows)
		{
			this.Rows = rows ?? throw new ArgumentNullException(nameof(rows));
		}

		public Dictionary<string, object>[] Rows { get; }
	}
}

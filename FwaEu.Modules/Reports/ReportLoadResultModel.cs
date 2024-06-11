using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public class ReportLoadResultModel<TModel>
		where TModel : class
	{
		public ReportLoadResultModel(TModel report)
		{
			this.Report = report
				?? throw new ArgumentNullException(nameof(report));
		}

		public TModel Report { get; }
	}
}

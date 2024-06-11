using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports
{
	public interface IReportProviderFactory
	{
		IReportProvider Create(IServiceProvider serviceProvider);
	}
}

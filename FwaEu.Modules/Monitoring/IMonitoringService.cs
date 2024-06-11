using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Monitoring
{
	public interface IMonitoringService
	{
		Task PingAsync();
	}
}

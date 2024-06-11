using System.Threading.Tasks;
using FwaEu.Fwamework.Application;
using FwaEu.Modules.Monitoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FwaEu.Fwamework.Monitoring
{
	[Route("[controller]")]
	[ApiController]
	public class MonitoringController : ControllerBase
	{
		[HttpGet(nameof(Ping))]
		[PingAllowApplication]
		public async Task Ping([FromServices]IMonitoringService monitoringService)
		{
			await monitoringService.PingAsync();
		}

		[AllowAnonymous]
		[HttpGet(nameof(ApplicationInfo))]
		public IActionResult ApplicationInfo([FromServices]IApplicationInfo applicationInfo)
		{
			return Ok(applicationInfo);
		}
	}
}
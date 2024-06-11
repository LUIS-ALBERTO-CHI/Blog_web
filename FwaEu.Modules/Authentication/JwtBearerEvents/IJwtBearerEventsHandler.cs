using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JwtBearerEvents
{
	public interface IJwtBearerEventsHandler
	{
		Task OnMessageReceivedAsync(MessageReceivedContext context);
	}
}

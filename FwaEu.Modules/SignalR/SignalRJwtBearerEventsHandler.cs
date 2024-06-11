using FwaEu.Modules.Authentication.JwtBearerEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SignalR
{
	public class SignalRJwtBearerEventsHandler : IJwtBearerEventsHandler
	{
		public Task OnMessageReceivedAsync(MessageReceivedContext context)
		{
			var accessToken = context.Request.Query["access_token"];

			if (!String.IsNullOrEmpty(accessToken))
			{
				context.Token = accessToken;
			}

			return Task.CompletedTask;
		}
	}
}

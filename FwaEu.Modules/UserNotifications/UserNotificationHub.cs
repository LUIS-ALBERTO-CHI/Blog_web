using FwaEu.Fwamework.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications
{
	[Authorize]
	public class UserNotificationHub : Hub<IUserNotificationClient>
	{
		private readonly ICultureResolver _cultureResolver;

		public static string GetGroupName(CultureInfo culture)
		{
			return culture.TwoLetterISOLanguageName;
		}

		public UserNotificationHub(ICultureResolver cultureResolver)
		{
			this._cultureResolver = cultureResolver
				?? throw new ArgumentNullException(nameof(cultureResolver));
		}

		private CultureInfo GetConnectionCulture()
		{
			var httpContext = this.Context.GetHttpContext();

			return this._cultureResolver.ResolveBestCulture(
				httpContext == null
				? default(string[]) // NOTE: null will return application default culture
				: new[] { httpContext.Request.Query["culture"].First() });
		}

		public override async Task OnConnectedAsync()
		{
			var connectionCulture = this.GetConnectionCulture();

			await this.Groups.AddToGroupAsync(
				this.Context.ConnectionId,
				GetGroupName(connectionCulture));

			await base.OnConnectedAsync();
		}
	}
}

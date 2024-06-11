using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserNotifications
{
	public interface IUserNotificationClient
	{
		[HubMethodName("Notified")]
		Task SendAsync(string notificationType, object model);
	}
}

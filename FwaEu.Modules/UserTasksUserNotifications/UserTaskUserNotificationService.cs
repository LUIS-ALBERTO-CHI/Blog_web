using FwaEu.Modules.UserNotifications;
using FwaEu.Modules.UserTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasksUserNotifications
{
	public class UserTaskUserNotificationService
	{
		public UserTaskUserNotificationService(
			IUserNotificationService userNotificationService,
			IServiceProvider serviceProvider,
			IUserTaskFactoryProvider userTaskFactoryProvider)
		{
			this.UserNotificationService = userNotificationService
				?? throw new ArgumentNullException(nameof(userNotificationService));

			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this.UserTaskFactoryProvider = userTaskFactoryProvider
				?? throw new ArgumentNullException(nameof(userTaskFactoryProvider));
		}

		public IUserNotificationService UserNotificationService { get; }

		protected IServiceProvider ServiceProvider { get; }
		protected IUserTaskFactoryProvider UserTaskFactoryProvider { get; }

		public async Task ExecuteUserTaskAsync(Type userTaskType, object parameters,
			IEnumerable<string> userIdentities, CancellationToken cancellationToken)
		{
			var method = this.GetType()
				.GetMethods()
				.Where(m => m.Name == nameof(ExecuteUserTaskAsync) && m.IsGenericMethod)
				.First();

			var task = (Task)method.MakeGenericMethod(userTaskType, typeof(object), typeof(object))
				.Invoke(this, new[] { parameters, userIdentities, cancellationToken });

			await task;
		}

		public async Task ExecuteUserTaskAsync<TUserTask, TParameters, TResult>(
			TParameters parameters, IEnumerable<string> userIdentities,
			CancellationToken cancellationToken)
		{
			var factory = this.UserTaskFactoryProvider.FindFactoryNoPerimeter(typeof(TUserTask));
			if (factory == null)
			{
				throw new ArgumentException($"Task '{typeof(TUserTask).Assembly}' is not registed in Startup.", nameof(TUserTask));
			}

			var userTask = factory.Create();
			var result = await userTask.ExecuteAsync<TParameters, TResult>(parameters, cancellationToken);

			await this.UserNotificationService.SendNotificationAsync("UserTask",
				new UserTaskNotifyModel<TResult>(factory.Name, result), userIdentities);
		}
	}

	public static class UserTaskUserNotificationServiceExtensions
	{
		public static Task ExecuteUserTaskAsync<TUserTask, TParameters, TResult>(
			this UserTaskUserNotificationService userTaskUserNotificationService,
			TParameters parameters, string userIdentity,
			CancellationToken cancellationToken)
		{
			return userTaskUserNotificationService.ExecuteUserTaskAsync
				<TUserTask, TParameters, TResult>(
				parameters, new[] { userIdentity }, cancellationToken);
		}
	}
}
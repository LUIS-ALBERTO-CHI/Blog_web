using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public static class UserTasksExtensions
	{
		public static void AddFwameworkModuleUserTasks(this IServiceCollection services)
		{
			services.AddTransient<IUserTaskFactoryProvider, DefaultUserTaskFactoryProvider>();

			// By default, user task is accessible
			services.AddScoped(typeof(IUserTaskAccessManager<>), typeof(AccessibleTaskAccessManager<>));
		}

		public static UserTaskServicesBuilder AddUserTask<TTask>(this IServiceCollection services, string taskName)
			where TTask : class, IUserTask
		{
			var taskType = typeof(TTask);

			services.AddTransient<IUserTaskFactory>(sp =>
				new DefaultUserTaskFactory(taskName, taskType, sp));

			services.AddTransient<TTask>();

			return new UserTaskServicesBuilder(taskType, services);
		}
	}

	public class UserTaskServicesBuilder
	{
		public UserTaskServicesBuilder(Type userTaskType, IServiceCollection serviceCollection)
		{
			this.UserTaskType = userTaskType
				?? throw new ArgumentNullException(nameof(userTaskType));

			this.ServiceCollection = serviceCollection
				?? throw new ArgumentNullException(nameof(serviceCollection));
		}

		public Type UserTaskType { get; }
		public IServiceCollection ServiceCollection { get; }
	}
}

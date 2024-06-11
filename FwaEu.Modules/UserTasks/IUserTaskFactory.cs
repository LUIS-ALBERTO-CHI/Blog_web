using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public interface IUserTaskFactory
	{
		string Name { get; }
		Type UserTaskType { get; }

		IUserTask Create();
	}

	public class DefaultUserTaskFactory : IUserTaskFactory
	{
		public DefaultUserTaskFactory(string name, Type userTaskType, IServiceProvider serviceProvider)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));

			this.UserTaskType = userTaskType
				?? throw new ArgumentNullException(nameof(userTaskType));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public string Name { get; }
		public Type UserTaskType { get; }

		private readonly IServiceProvider _serviceProvider;

		public IUserTask Create()
		{
			return (IUserTask)this._serviceProvider.GetRequiredService(this.UserTaskType);
		}
	}
}

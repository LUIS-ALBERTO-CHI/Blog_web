using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasksUserNotifications
{
	public class UserTaskNotifyModel<TResult>
	{
		public UserTaskNotifyModel(string userTaskName, TResult result)
		{
			this.UserTaskName = userTaskName
				?? throw new ArgumentNullException(nameof(userTaskName));

			this.Result = result;
		}

		public string UserTaskName { get; }
		public TResult Result { get; }
	}
}

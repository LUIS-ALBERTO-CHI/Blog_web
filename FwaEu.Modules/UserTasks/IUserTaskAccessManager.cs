using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.UserTasks
{
	public interface IUserTaskAccessManager
	{
		Task<bool> IsAccessibleAsync();
	}

	public interface IUserTaskAccessManager<TTask> : IUserTaskAccessManager
		where TTask : class, IUserTask
	{
		
	}

	public class AccessibleTaskAccessManager<TTask> : IUserTaskAccessManager<TTask>
		where TTask : class, IUserTask
	{
		public Task<bool> IsAccessibleAsync()
		{
			return Task.FromResult(true);
		}
	}
}

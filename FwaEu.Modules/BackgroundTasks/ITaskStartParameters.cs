using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.BackgroundTasks
{
	public interface ITaskStartParameters
	{
		string TaskName { get; }
		string Argument { get; }
	}

	public class TaskStartParameters : ITaskStartParameters
	{
		public TaskStartParameters(string taskName, string argument)
		{
			this.TaskName = taskName ?? throw new ArgumentNullException(nameof(taskName));
			this.Argument = argument;
		}

		public string TaskName { get; }
		public string Argument { get; }
	}
}

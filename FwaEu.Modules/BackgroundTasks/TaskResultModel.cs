using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.BackgroundTasks
{
	public class TaskResultModel
	{
		public TaskResultModel(string data, string errorMessage)
		{
			this.Data = data;
			this.ErrorMessage = errorMessage;
		}

		public string Data { get; }
		public string ErrorMessage { get; }
	}
}

using System;

namespace FwaEu.Modules.BackgroundTasks
{
	public class BackgroundTaskExecutionException : Exception
	{
		public BackgroundTaskExecutionException(string message) : base(message)
		{
		}

		public BackgroundTaskExecutionException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

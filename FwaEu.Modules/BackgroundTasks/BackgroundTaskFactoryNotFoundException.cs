using System;

namespace FwaEu.Modules.BackgroundTasks
{
	public class BackgroundTaskFactoryNotFoundException : Exception
	{
		public BackgroundTaskFactoryNotFoundException(string message) : base(message)
		{
		}

		public BackgroundTaskFactoryNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

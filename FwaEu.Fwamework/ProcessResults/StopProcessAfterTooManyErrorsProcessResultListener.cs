using System;

namespace FwaEu.Fwamework.ProcessResults
{
	public class StopProcessAfterTooManyErrorsProcessResultListener : IProcessResultListener
	{
		public static void RemoveListener(ProcessResult processResult, ProcessStoppedAfterTooManyErrorsException exception)
		{
			if (!(processResult.Listener is StopProcessAfterTooManyErrorsProcessResultListener))
			{
				throw new NotSupportedException(
					$"Method {nameof(RemoveListener)} is invokable only with a listener of type {nameof(StopProcessAfterTooManyErrorsProcessResultListener)}.");
			}

			processResult.Listener = null;
			processResult.CreateContext("Process stopped.", "StopProcessAfterTooManyErrors")
				.Add(ErrorProcessResultEntry.FromException(exception));
		}

		public StopProcessAfterTooManyErrorsProcessResultListener(int maximumErrorCount = 200)
		{
			this._maximumErrorCount = maximumErrorCount;
		}

		private int _maximumErrorCount;
		private int _errorCount;

		public void OnResultEntryAdded(ProcessResultEntry entry, ProcessResultContext context)
		{
			if (entry is ErrorProcessResultEntry)
			{
				this._errorCount++;

				if (this._errorCount >= this._maximumErrorCount)
				{
					throw new ProcessStoppedAfterTooManyErrorsException();
				}
			}
		}

		public void OnResultContextAdded(ProcessResultContext context)
		{
		}
	}

	public class ProcessStoppedAfterTooManyErrorsException : Exception
	{
		public ProcessStoppedAfterTooManyErrorsException()
			: base("Process was stopped because there were too many errors.")
		{
		}
	}
}

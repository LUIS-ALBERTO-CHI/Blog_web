using System;

namespace FwaEu.Fwamework.ProcessResults
{
	public class ProcessResultEntryErrorException : Exception
	{
		public ProcessResultEntryErrorException(string message) : base(message)
		{
		}

		public ProcessResultEntryErrorException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

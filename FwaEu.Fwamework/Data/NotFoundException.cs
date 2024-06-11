using System;

namespace FwaEu.Fwamework.Data
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message)
		{
		}

		public NotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
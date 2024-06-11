using System;

namespace FwaEu.Fwamework.Data
{
	public class ReferenceNotFoundException : Exception
	{
		public ReferenceNotFoundException(string message)
			: base(message)
		{
		}

		public ReferenceNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

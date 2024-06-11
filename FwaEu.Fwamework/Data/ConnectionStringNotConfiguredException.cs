using System;

namespace FwaEu.Fwamework.Data
{
	public class ConnectionStringNotConfiguredException : Exception
	{
		public ConnectionStringNotConfiguredException(string message) : base(message)
		{
		}

		public ConnectionStringNotConfiguredException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

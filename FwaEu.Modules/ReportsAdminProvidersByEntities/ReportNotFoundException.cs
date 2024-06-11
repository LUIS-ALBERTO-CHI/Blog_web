using System;

namespace FwaEu.Modules.ReportsAdminProvidersByEntities
{
	public class ReportNotFoundException : Exception
	{
		public ReportNotFoundException(string message) : base(message)
		{
		}

		public ReportNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

using System.Collections.Generic;

namespace FwaEu.Fwamework.WebApi
{
	public class AllowedApplicationsOptions
	{
		public Dictionary<string, ApplicationEntry> Applications { get; set; }
	}

	public class ApplicationEntry
	{
		public string Secret { get; set; }
		public Filter Filter { get; set; }
	}

	public class Filter
	{
		public string[] Allowed { get; set; }
	}
}

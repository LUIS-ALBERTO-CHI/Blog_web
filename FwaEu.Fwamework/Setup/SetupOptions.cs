using FwaEu.Fwamework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class SetupOptions
	{
		public PathEntry[] BeforeUpdateSchemaPaths { get; set; }
		public PathEntry[] AfterUpdateSchemaPaths { get; set; }
		public PathEntry[] BrowsableImportableFilesPaths { get; set; }

		public SecuritySetupOptions Security { get; set; }
	}

	public class SecuritySetupOptions
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public int ExpirationDelayInMinutes { get; set; }
		public string TokenSigningKey { get; set; } = "aidlbiBhaSBtYXJyZSBkZSBjZSBidWcgZGUgZGluZGU=";
	}
}

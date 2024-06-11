using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.StaticFiles
{
	public class FwameworkFileExtensionContentTypeProvider : FileExtensionContentTypeProvider
	{
		public FwameworkFileExtensionContentTypeProvider()
		{
			this.AddFwameworkMappings();
		}

		public FwameworkFileExtensionContentTypeProvider(IDictionary<string, string> mapping)
			: base(mapping)
		{
			this.AddFwameworkMappings();
		}

		private void AddFwameworkMappings()
		{
			this.Mappings.Add(".sql", "application/sql");

			this.Mappings.Remove(".csv");
			this.Mappings.Add(".csv", "text/csv");
		}
	}
}

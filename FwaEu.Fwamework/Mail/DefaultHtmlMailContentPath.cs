using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Mail
{
	public class DefaultHtmlMailContentPath : IHtmlMailContentPath
	{
		public DefaultHtmlMailContentPath(IOptions<HtmlMailContentPathOptions> settings)
		{
			this.CompanyLogo = Environment.CurrentDirectory + settings?.Value?.CompanyLogo;
			this.FWALogo = Environment.CurrentDirectory + settings?.Value?.FWALogo;
		}

		public string CompanyLogo { get; set; }
		public string FWALogo { get ; set ; }
	}
}

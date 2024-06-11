using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace FwaEu.Modules.Mail.HtmlMailSender
{
	public interface IHtmlMailParametersProvider
	{
		void EnrichParameters(Dictionary<string, object> viewBag);
	}
}

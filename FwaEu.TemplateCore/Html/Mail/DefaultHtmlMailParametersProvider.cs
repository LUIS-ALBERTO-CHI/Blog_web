using FwaEu.Fwamework.Application;
using FwaEu.Modules.Mail.HtmlMailSender;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Html.Mail
{
	public class DefaultHtmlMailParametersProvider : IHtmlMailParametersProvider
	{
		private readonly IApplicationInfo _applicationInfo;

		public DefaultHtmlMailParametersProvider(IApplicationInfo applicationInfo)
		{
			_applicationInfo = applicationInfo ?? throw new ArgumentNullException(nameof(applicationInfo));
		}

		public void EnrichParameters(Dictionary<string, object> parameters)
		{
			parameters["ApplicationName"] = _applicationInfo.Name;
			parameters["Version"] = _applicationInfo.Version;
			parameters["CopyrightYears"] = _applicationInfo.CopyrightYears;
		}
	}
}

using FwaEu.Fwamework.Mail;
using FwaEu.Modules.Mail.HtmlMailSender;
using System;
using System.Collections.Generic;

namespace FwaEu.BlogWeb.Html.Mail
{
	public class DefaultHtmlMailContentPathProvider : IHtmlMailContentPathProvider
	{
		private readonly IHtmlMailContentPath _htmlMailContentPath;
		public DefaultHtmlMailContentPathProvider(IHtmlMailContentPath htmlMailContentPath)
		{
			_htmlMailContentPath = htmlMailContentPath ?? throw new ArgumentNullException(nameof(htmlMailContentPath)); ;
		}

		public IEnumerable<ContentModel> GetAllContents()
		{
			yield return new ContentModel("CompanyLogoPath", this._htmlMailContentPath.CompanyLogo);
			yield return new ContentModel("FWALogoPath", this._htmlMailContentPath.FWALogo);
		}

		public void EnrichParameters(Dictionary<string, object> parameters)
		{
			foreach (var content in this.GetAllContents())
			{
				parameters[content.Id] = content.Path;
			}
		}
	}
}

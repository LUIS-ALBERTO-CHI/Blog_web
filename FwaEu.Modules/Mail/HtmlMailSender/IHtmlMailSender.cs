using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.HtmlMailSender
{
	public interface IHtmlMailSender<TModel>
	{
		Task SendHtmlMailAsync(TModel model, Action<MimeMessage> configure);
		Task SendHtmlMailAsync(TModel model, Action<BodyBuilder> configureBodyBuilder, Action<MimeMessage> configure);
	}
}

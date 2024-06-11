using FwaEu.Fwamework.Mail;
using FwaEu.Modules.HtmlRenderer;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.HtmlMailSender
{
	public class DefaultHtmlMailSender<TModel> : IHtmlMailSender<TModel>
	{
		private readonly MailSender _mailSender;
		private readonly IHtmlRenderer<TModel> _htmlRenderer;
		private readonly IHtmlMailParametersProvider _htmlMailParametersProvider;
		private readonly IHtmlMailContentPathProvider _htmlMailContentPathProvider;

		public DefaultHtmlMailSender(MailSender mailSender, IHtmlRenderer<TModel> htmlRenderer,
			IHtmlMailParametersProvider htmlMailParametersProvider = null,
			IHtmlMailContentPathProvider htmlMailContentPathProvider = null)
		{
			_mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
			_htmlRenderer = htmlRenderer ?? throw new ArgumentNullException(nameof(htmlRenderer));
			_htmlMailParametersProvider = htmlMailParametersProvider;
			_htmlMailContentPathProvider = htmlMailContentPathProvider;
		}

		public async Task SendHtmlMailAsync(TModel model, Action<MimeMessage> configure)
		{
			await SendHtmlMailAsync(model, null, configure);
		}

		protected IDictionary<string, object> GetParameters()
		{
			var parameters = new Dictionary<string, object>();

			if (_htmlMailParametersProvider != null)
			{
				_htmlMailParametersProvider.EnrichParameters(parameters);
			}
			return parameters;
		}

		protected void AttachContentsToBodyBuilder(BodyBuilder bodyBuilder, IDictionary<string, object> parameters)
		{
			if (this._htmlMailContentPathProvider != null)
			{
				foreach (var content in this._htmlMailContentPathProvider.GetAllContents())
				{
					var image = bodyBuilder.LinkedResources.Add(content.Path);

					image.ContentId = content.Id;
					parameters.Add(content.Id, content.Id);
				}
			}
		}

		public async Task SendHtmlMailAsync(TModel model, Action<BodyBuilder> configureBodyBuilder, Action<MimeMessage> configure)
		{
			var parameters = this.GetParameters();

			var builder = new BodyBuilder();

			this.AttachContentsToBodyBuilder(builder, parameters);

			builder.HtmlBody = await _htmlRenderer.RenderAsync(model, parameters);

			if (configureBodyBuilder != null)
			{
				configureBodyBuilder(builder);
			}

			var message = new MimeMessage
			{
				Body = builder.ToMessageBody()
			};

			configure(message);

			await this._mailSender.SendAsync(message);
		}
	}
}

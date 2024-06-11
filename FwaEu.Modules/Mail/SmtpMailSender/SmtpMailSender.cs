using FwaEu.Fwamework.Mail;
using FwaEu.Fwamework.Mail.EventListener;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.SmtpMailSender
{
	public class SmtpMailSender : MailSender
	{
		public SmtpMailSender(IOptions<SmtpOptions> settings, IMailListener mailListener) :	base(mailListener)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		private readonly IOptions<SmtpOptions> _settings;
		protected override async Task DoSendAsync(MimeMessage message)
		{
			var settings = this._settings.Value;

			using (var client = new SmtpClient())
			{
				//TODO: Ignoring SSL validation certificate errors  https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/7995/
				if (settings.IgnoreSSLCertificateValidation)
				{
					client.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
				}
				//NOTE: We need to force the SecureSocketOptions because the "Auto" option doesn't works for Office365
				//https://github.com/jstedfast/MailKit/blob/master/FAQ.md#SslHandshakeException
				client.Connect(settings.Host, settings.Port, settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
				if (!String.IsNullOrEmpty(settings.UserName))
				{
					client.Authenticate(settings.UserName, settings.Password);
				}
				message.From.Add(MailboxAddress.Parse(settings.FromAddress));

				await client.SendAsync(message);
				await client.DisconnectAsync(true);
			}
		}
	}
}

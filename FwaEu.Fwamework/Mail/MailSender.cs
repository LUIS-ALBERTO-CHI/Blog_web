using FwaEu.Fwamework.Mail.EventListener;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Mail
{
	public abstract class MailSender
	{
		private readonly IMailListener _mailListener;

		public MailSender(IMailListener mailListener)
		{
			_mailListener = mailListener ?? throw new ArgumentNullException(nameof(mailListener));
		}
		protected abstract Task DoSendAsync(MimeMessage message);

		public async Task SendAsync(MimeMessage message)
		{
			_mailListener.OnBeforeSend(message);
			await this.DoSendAsync(message);
			_mailListener.OnAfterSend(message);
		}
	}
}

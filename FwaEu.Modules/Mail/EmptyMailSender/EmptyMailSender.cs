using FwaEu.Fwamework.Mail;
using FwaEu.Fwamework.Mail.EventListener;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.EmptyMailSender
{
	public class EmptyMailSender : MailSender
	{
		public EmptyMailSender(IMailListener mailListener) : base(mailListener)
		{
		}

		protected override Task DoSendAsync(MimeMessage message)
		{
			return Task.CompletedTask; //NOTE: No action
		}
	}
}

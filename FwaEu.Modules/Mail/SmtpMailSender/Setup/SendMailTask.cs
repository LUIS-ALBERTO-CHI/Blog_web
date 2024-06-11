using FwaEu.Fwamework.Mail;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.SmtpMailSender.Setup
{
	public class SendMailTask : SetupTask<SendTaskModel>
	{
		public SendMailTask(MailSender mailSender)
		{
			this._mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
		}

		private readonly MailSender _mailSender;

		public override string Name => "SendMail";

		public override async Task<NoDataSetupTaskResult> ExecuteAsync(SendTaskModel arguments)
		{
			var message = new MimeMessage()
			{
				Subject = "Test email from Setup.",
				Body = new TextPart("plain") 
				{ 
					Text = "Test email from Setup (body)." 
				}
			};

			message.To.Add(MailboxAddress.Parse(arguments.RecipientAddress));

			var processResult = new ProcessResult();
			var context = processResult.CreateContext($"Sending test mail to {arguments.RecipientAddress}", "SendMailSetupTask");

			try
			{
				await this._mailSender.SendAsync(message);
			}
			catch (Exception ex)
			{
				context.Add(ErrorProcessResultEntry.FromException(ex));
			}

			if (context.Entries.Length == 0)
			{
				context.Add(new InfoProcessResultEntry("Mail sent."));
			}

			return new NoDataSetupTaskResult(processResult);
		}
	}

	public class SendTaskModel
	{
		public string RecipientAddress { get; set; }
	}
}

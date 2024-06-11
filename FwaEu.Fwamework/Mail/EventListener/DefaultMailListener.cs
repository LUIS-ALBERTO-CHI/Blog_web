using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Mail.EventListener
{
	public class DefaultMailListener : IMailListener
	{ 
		private readonly ILogger _logger;

		public DefaultMailListener(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<DefaultMailListener>();
		}

		public void OnBeforeSend(MimeMessage message)
		{
			_logger.LogDebug("Preparing email, subject: {0}", message.Subject);
		}

		public void OnAfterSend(MimeMessage message)
		{
			_logger.LogDebug("Email sent, subject: {0}.", message.Subject);
		}
	}
}

using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Mail.EventListener
{
	public interface IMailListener
	{
		void OnBeforeSend(MimeMessage message);
		void OnAfterSend(MimeMessage message);
	}
}

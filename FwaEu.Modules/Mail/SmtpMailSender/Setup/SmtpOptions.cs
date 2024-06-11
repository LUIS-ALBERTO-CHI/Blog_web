using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.SmtpMailSender.Setup
{
	public class SmtpOptions
	{
		public SmtpOptions(string host, int port,
			string userName, bool hasPassword,
			bool enableSsl, string fromAddress, bool ignoreSSLCertificateValidation)
		{
			this.Host = host;
			this.Port = port;
			this.UserName = userName;
			this.HasPassword = hasPassword;
			this.EnableSsl = enableSsl;
			this.FromAddress = fromAddress;
			this.IgnoreSSLCertificateValidation = ignoreSSLCertificateValidation;
		}

		public string Host { get; }
		public int Port { get; }
		public string UserName { get; }
		public bool HasPassword { get; }
		public bool EnableSsl { get; }
		public string FromAddress { get; }
		public bool IgnoreSSLCertificateValidation { get; }
	}
}

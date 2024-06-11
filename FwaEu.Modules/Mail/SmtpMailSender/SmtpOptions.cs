namespace FwaEu.Modules.Mail.SmtpMailSender
{
	public class SmtpOptions
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool EnableSsl { get; set; }
		public string FromAddress { get; set; }
		public bool IgnoreSSLCertificateValidation { get; set; }
	}
}

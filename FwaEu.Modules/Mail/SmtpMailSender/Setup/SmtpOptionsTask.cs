using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Mail.SmtpMailSender.Setup
{
	public class SmtpOptionsTask : ISetupTask
	{
		public string Name => "SmtpOptions";
		public Type ArgumentsType => null;

		public SmtpOptionsTask(IOptions<Mail.SmtpMailSender.SmtpOptions> settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

		private readonly IOptions<Mail.SmtpMailSender.SmtpOptions> _settings;

		public Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var currentValue = this._settings.Value;

			var model = new SmtpOptions(
				host: currentValue.Host,
				port: currentValue.Port,
				userName: currentValue.UserName,
				hasPassword: !string.IsNullOrEmpty(currentValue.Password),
				enableSsl: currentValue.EnableSsl,
				fromAddress: currentValue.FromAddress,
				ignoreSSLCertificateValidation: currentValue.IgnoreSSLCertificateValidation
				);

			return Task.FromResult<ISetupTaskResult>(
				new SetupTaskResult<SmtpOptions>(new ProcessResult(), model));
		}
	}

}

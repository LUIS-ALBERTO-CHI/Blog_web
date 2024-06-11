using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FwaEu.Fwamework.Setup.WebApi
{
	public class SetupCredentialsApiModel
	{
		public SetupCredentialsApiModel([Required] string login, [Required] string password)
		{
			Login = login ?? throw new ArgumentNullException(nameof(login));
			Password = password ?? throw new ArgumentNullException(nameof(password));
		}

		public string Login { get; }
		public string Password { get; }
	}
}

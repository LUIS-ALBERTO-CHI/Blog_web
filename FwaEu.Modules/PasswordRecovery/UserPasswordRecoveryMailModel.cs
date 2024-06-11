using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.PasswordRecovery
{
	public class UserPasswordRecoveryMailModel
	{
		public string Greetings { get; set; }
		public string Message { get; set; }
		public string Title { get; set; }
		public string Link { get; set; }
	}
}

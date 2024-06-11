using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.WebApi
{
	public class CredentialsApiModel
	{
		[Required]
		public string Identity { get; set; }

		[Required]
		public string Password { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace FwaEu.Modules.PasswordRecovery.WebApi
{
	public class UserPasswordRecoveryModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FwaEu.Modules.PasswordRecovery.WebApi
{
	public class RequestPasswordRecoveryModel
	{
		[Required]
		public int? UserId { get; set; }

		[Required]
		public Guid? Guid { get; set; }

		[Required]
		public string NewPassword { get; set; }
	}
}

using FwaEu.Fwamework.Users;
using System.ComponentModel.DataAnnotations;

namespace FwaEu.Modules.Users.AdminState.Parts
{
	public class AdminStatePartModel
	{
		public bool? IsAdmin { get; set; }

		[Required]
		public UserState? State { get; set; }
	}
}

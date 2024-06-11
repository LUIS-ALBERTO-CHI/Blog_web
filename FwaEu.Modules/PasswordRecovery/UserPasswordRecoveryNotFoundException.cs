using FwaEu.Fwamework.Data;

namespace FwaEu.Modules.PasswordRecovery
{
	public class UserPasswordRecoveryNotFoundException : NotFoundException
	{
		public UserPasswordRecoveryNotFoundException(string message) : base(message)
		{
		}
	}
}

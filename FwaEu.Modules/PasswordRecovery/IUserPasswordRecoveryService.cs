using System;
using System.Threading.Tasks;


namespace FwaEu.Modules.PasswordRecovery
{
	
	public interface IUserPasswordRecoveryService
	{
		Task ReinitializePasswordAsync(string email);
		Task UpdatePasswordAsync(RequestPasswordRecoveryModel model);
	}
}

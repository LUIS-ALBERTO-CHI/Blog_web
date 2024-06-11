using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public interface IUserDetailsService
	{
		Task<object> GetUserDetailsAsync(int userId);
	}
}

using FwaEu.Fwamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public interface IUserPerimeterProvider
	{
		Task<bool> HasFullAccessAsync(int userId);
		Task<object[]> GetAccessibleIdsAsync(int userId);
		Task UpdatePerimeterAsync(int userId, bool hasFullAccess, object[] accessibleIds);
	}
}

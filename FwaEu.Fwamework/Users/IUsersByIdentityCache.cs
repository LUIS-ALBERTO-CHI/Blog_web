using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public interface IUsersByIdentityCache
	{
		Task<IEnumerable<UserByIdentityCacheModel>> LoadAsync(IQueryable<UserEntity> query);
	}

	public class UserByIdentityCacheModel
	{
		public int UserId { get; set; }
		public string Identity { get; set; }
	}
}

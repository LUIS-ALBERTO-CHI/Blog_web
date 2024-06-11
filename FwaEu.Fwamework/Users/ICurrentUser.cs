using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public interface ICurrentUser
	{
		string Identity { get; }
		UserEntity Entity { get; }
	}

	public class CurrentUser : ICurrentUser
	{
		public CurrentUser(string identity, UserEntity entity)
		{
			this.Identity = identity ?? throw new ArgumentNullException(nameof(identity));
			this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
		}

		public string Identity { get; }
		public UserEntity Entity { get; }
	}
}

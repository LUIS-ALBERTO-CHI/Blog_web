using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.Avatars
{
	public interface IAvatarUser : IPerson
	{
		int Id { get; }
		string Email { get; }
	}
}

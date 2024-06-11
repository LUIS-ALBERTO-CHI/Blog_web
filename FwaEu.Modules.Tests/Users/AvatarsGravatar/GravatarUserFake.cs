using FwaEu.Modules.Users.Avatars;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.Users.AvatarsGravatar
{
	public class GravatarUserFake : IAvatarUser
	{
		public GravatarUserFake(string email)
		{
			this.Email = email ?? throw new ArgumentNullException(nameof(email));
		}

		public string Email { get; }

		public int Id => throw new NotImplementedException();
		public string FirstName => throw new NotImplementedException();
		public string LastName => throw new NotImplementedException();
	}
}

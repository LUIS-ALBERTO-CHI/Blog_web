using FwaEu.Modules.Users.Avatars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.MasterData
{
	public class EntityUserMasterDataModel : IAvatarUser
	{
		public EntityUserMasterDataModel(int id, string firstName, string lastName, string email)
		{
			this.Id = id;
			this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			this.Email = email ?? throw new ArgumentNullException(nameof(email));
		}

		public int Id { get; }
		public string FirstName { get; }
		public string LastName { get; }
		public string Email { get; }
		public string AvatarUrl { get; set; }
	}
}

using FwaEu.Modules.Users.Avatars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users.MasterData
{
	public class UserMasterDataModel
	{
		public UserMasterDataModel(int id, string firstName, string lastName, string avatarUrl)
		{
			this.Id = id;
			this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			
			this.AvatarUrl = avatarUrl; //NOTE: Null allowed, there will be a fallback on client side
		}

		public int Id { get; }
		public string FirstName { get; }
		public string LastName { get; }
		public string AvatarUrl { get; }
	}
}

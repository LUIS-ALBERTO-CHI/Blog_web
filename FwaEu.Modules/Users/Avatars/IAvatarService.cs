using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.Avatars
{
	public interface IAvatarService
	{
		Task<Avatar> GetAvatarAsync(IAvatarUser user);
	}

	public class Avatar
	{
		public Avatar(string url)
		{
			this.Url = url ?? throw new ArgumentNullException(nameof(url));
		}

		public string Url { get; }
	}
}

using FwaEu.Fwamework.Security;
using FwaEu.Modules.Users.Avatars;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.AvatarsGravatar
{
	public class GravatarAvatarService : IAvatarService
	{
		public GravatarAvatarService(IOptions<GravatarOptions> options)
		{
			this._options = options ?? throw new ArgumentNullException(nameof(options));
		}

		private readonly IOptions<GravatarOptions> _options;

		public Task<Avatar> GetAvatarAsync(IAvatarUser user)
		{
			var hashedEmail = string.IsNullOrEmpty(user.Email) ? string.Empty : MD5Hasher.ToMD5String(user.Email);
			var urlFormat = this._options.Value.UrlFormat;

			var avatar = new Avatar(String.Format(urlFormat, hashedEmail));

			return Task.FromResult(avatar);
		}
	}
}

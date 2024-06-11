using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.AvatarsGravatar
{
	public class GravatarOptions
	{
		/// <summary>
		/// Placeholder {0} will be fill with MD5(email)
		/// </summary>
		public string UrlFormat { get; set; }
			= "https://www.gravatar.com/avatar/{0}?size=200&default=robohash&rating=g";

		//NOTE: Documentation on https://fr.gravatar.com/site/implement/images/
	}
}
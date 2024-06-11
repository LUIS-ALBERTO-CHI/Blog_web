using FwaEu.Modules.Users.AvatarsGravatar;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.Users.AvatarsGravatar
{
	[TestClass]
	public class GravatarAvatarServiceTests
	{
		[TestMethod]
		[DataRow("thomas.rosel@fwa.eu",
			"https://www.gravatar.com/avatar/{0}",
			"https://www.gravatar.com/avatar/af23151e5e4faa2485f40f2f30d54b1a")]
		[DataRow("thomas.rosel@fwa.eu",
			"https://www.gravatar.com/avatar/{0}?size=200&default=robohash&rating=g",
			"https://www.gravatar.com/avatar/af23151e5e4faa2485f40f2f30d54b1a?size=200&default=robohash&rating=g")]
		public async Task GetAvatarAsync(string email, string urlFormat, string expectedUrl)
		{
			var settings = new GravatarOptionsFake(
				new GravatarOptions()
				{
					UrlFormat = urlFormat
				});

			var service = new GravatarAvatarService(settings);
			var user = new GravatarUserFake(email);

			var avatar = await service.GetAvatarAsync(user);
			Assert.AreEqual(expectedUrl, avatar.Url);
		}
	}
}

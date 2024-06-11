using FwaEu.Modules.Users.AvatarsGravatar;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.Users.AvatarsGravatar
{
	public class GravatarOptionsFake : IOptions<GravatarOptions>
	{
		public GravatarOptionsFake(GravatarOptions value)
		{
			Value = value
				?? throw new ArgumentNullException(nameof(value));
		}

		public GravatarOptions Value { get; }

		public GravatarOptions Get(string name)
		{
			throw new NotImplementedException();
		}

		public IDisposable OnChange(Action<GravatarOptions, string> listener)
		{
			throw new NotImplementedException();
		}
	}
}

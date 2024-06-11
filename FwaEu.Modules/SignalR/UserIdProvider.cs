using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Modules.SignalR
{
	public class UserIdProvider : Microsoft.AspNetCore.SignalR.IUserIdProvider
	{
		private readonly IIdentityResolver _identityResolver;
		private readonly IEnumerable<IIdentityResolver> _identityResolvers;

		public UserIdProvider(IEnumerable<IIdentityResolver> identityResolvers)
		{
			this._identityResolvers = identityResolvers
				?? throw new ArgumentNullException(nameof(identityResolvers));
		}

		public string GetUserId(HubConnectionContext connection)
		{
			var identity = this._identityResolvers.Select(ir => ir.ResolveIdentity(connection.User)).First(i => i != null);
			return identity;
		}
	}
}

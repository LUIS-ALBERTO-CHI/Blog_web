using System;

namespace FwaEu.Modules.GenericAdmin
{
	public class AuthorizationException : Exception
	{
		public AuthorizationException(string actionName)
			: base($"Action '{actionName}' not authorized.")
		{
		}
	}
}
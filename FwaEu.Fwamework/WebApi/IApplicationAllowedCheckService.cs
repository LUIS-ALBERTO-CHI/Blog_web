using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.WebApi
{
	public interface IApplicationAllowedCheckService
	{
		bool IsApplicationAllowed(string applicationName, string secret, string ip);
		bool IsSecretMatchWithAnyApplication(string secret, string ip);
	}
}

using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.Authentication.WebApi
{
	public class SetupTokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
	{
		public const string SchemeName = "SetupTokenScheme";
		public string Scheme => SchemeName;
	}
}

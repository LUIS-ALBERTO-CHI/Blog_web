using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	/// <summary>
	/// Implements this interface on class map you want to exclude from
	/// automatic class map discovery. You will have to register manually
	/// these classmap (usually in you IAuthenticationInitializer implementation).
	/// </summary>
	public interface IAuthenticationClassMap
	{
	}
}

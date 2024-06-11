using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public interface IPasswordHasher
	{
		string Hash(string password);
	}
}

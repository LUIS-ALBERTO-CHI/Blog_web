using FwaEu.Fwamework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public class UserNotFoundException : NotFoundException
	{
		public UserNotFoundException(string message) : base(message)
		{
		}

		public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

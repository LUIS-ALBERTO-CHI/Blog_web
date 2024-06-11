using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.AdminState.Parts
{
	public class NonAdministratorCreateOrEditAdminUserException : Exception
	{
		public NonAdministratorCreateOrEditAdminUserException()
		{
		}

		public NonAdministratorCreateOrEditAdminUserException(string message) : base(message)
		{
		}

		public NonAdministratorCreateOrEditAdminUserException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NonAdministratorCreateOrEditAdminUserException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}

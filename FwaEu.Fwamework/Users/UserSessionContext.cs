using System;
using System.Linq;
using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.Fwamework.Users
{
	public interface IUserSessionContextSessionContextProvider
	{
		BaseSessionContext<IStatefulSessionAdapter> SessionContext { get; }
	}

	public class DefaultUserSessionContextSessionContextProvider : IUserSessionContextSessionContextProvider
	{
		public DefaultUserSessionContextSessionContextProvider(MainSessionContext sessionContext)
		{
			this.SessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public BaseSessionContext<IStatefulSessionAdapter> SessionContext { get; }
	}

	public class UserSessionContext
	{
		public UserSessionContext(IUserSessionContextSessionContextProvider userSessionContextSessionContextProvider)
		{
			this.SessionContext = (userSessionContextSessionContextProvider
				?? throw new ArgumentNullException(nameof(userSessionContextSessionContextProvider)))
				.SessionContext;
		}

		public BaseSessionContext<IStatefulSessionAdapter> SessionContext { get; }
		public UserEditModel LoadingModel { get; set; }
		public UserEntity SaveUserEntity { get; set; }
	}
}

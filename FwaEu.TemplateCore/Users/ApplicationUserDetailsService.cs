using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users
{
	public class ApplicationUserDetailsService : IUserDetailsService
	{
		public ApplicationUserDetailsService(IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory)
		{
			this._repositorySessionFactory = repositorySessionFactory
				?? throw new ArgumentNullException(nameof(repositorySessionFactory));
		}

		private readonly IRepositorySessionFactory<IStatefulSessionAdapter> _repositorySessionFactory;

		public async Task<object> GetUserDetailsAsync(int userId)
		{
			using (var repositorySession = this._repositorySessionFactory.CreateSession())
			{
				var repository = repositorySession.Create<ApplicationUserEntityRepository>();
				var user = await repository.GetAsync(userId);

				return new ApplicationUserDetails(user.Email);
			}
		}
	}

	public class ApplicationUserDetails
	{
		public ApplicationUserDetails(string email)
		{
			this.Email = email ?? throw new ArgumentNullException(nameof(email));
		}

		public string Email { get; }
	}
}

using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IRepositoryDataFilter<TEntity, TIdentifier>
		where TEntity : class
	{
		Task<bool> AcceptAsync(TEntity entity, RepositoryDataFilterContext<TEntity, TIdentifier> context);
		Expression<Func<TEntity,bool>> Accept(RepositoryDataFilterContext<TEntity, TIdentifier> context);
	}

	public class RepositoryDataFilterContext<TEntity, TIdentifier>
		where TEntity : class
	{
		public RepositoryDataFilterContext(IRepository<TEntity, TIdentifier> repository, ISessionAdapter session, IServiceProvider serviceProvider)
		{
			this.Repository = repository
				?? throw new ArgumentNullException(nameof(repository));

			this.Session = session
				?? throw new ArgumentNullException(nameof(session));

			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public IRepository<TEntity, TIdentifier> Repository { get; }
		public ISessionAdapter Session { get; }
		public IServiceProvider ServiceProvider { get; }
	}
}

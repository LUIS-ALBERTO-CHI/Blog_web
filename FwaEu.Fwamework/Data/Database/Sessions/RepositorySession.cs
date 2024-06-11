using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public interface IRepositorySession : IDisposable
	{
		ISessionAdapter Session { get; }
		IRepositoryFactory RepositoryFactory { get; }
	}

	public static class RepositorySessionExtensions
	{
		public static TRepository Create<TRepository>(this IRepositorySession repositorySession)
		{
			return repositorySession.RepositoryFactory.Create<TRepository>(repositorySession.Session);
		}

		public static IRepository CreateByEntityType(this IRepositorySession repositorySession, Type entityType)
		{
			return repositorySession.RepositoryFactory.CreateByEntityType(entityType,
				repositorySession.Session);
		}

		public static Task<TEntity> GetOrNotFoundExceptionAsync<TEntity, TIdentifier, TRepository>(
			this IRepositorySession repositorySession, TIdentifier? id)
			where TEntity : class
			where TIdentifier : struct
			where TRepository : IRepository<TEntity, TIdentifier>
		{
			return GetOrExceptionAsync<TEntity, TIdentifier, TRepository>(repositorySession, id,
				() => new NotFoundException($"Entity not found with id #{id.Value}."));
		}

		public static async Task<TEntity> GetOrExceptionAsync<TEntity, TIdentifier, TRepository>(
			this IRepositorySession repositorySession, TIdentifier? id,
			Func<Exception> notFoundExceptionFactory)
			where TEntity : class
			where TIdentifier : struct
			where TRepository : IRepository<TEntity, TIdentifier>
		{
			if (id == null)
			{
				return null;
			}

			var entity = await repositorySession
				.Create<TRepository>()
				.GetAsync(id.Value);

			if (entity == null)
			{
				throw notFoundExceptionFactory();
			}

			return entity;
		}
	}

	public class RepositorySession<TSessionAdapter> : IRepositorySession
		where TSessionAdapter : class, ISessionAdapter
	{
		public RepositorySession(TSessionAdapter session, IRepositoryFactory repositoryFactory)
		{
			this.Session = session
				?? throw new ArgumentNullException(nameof(session));

			this.RepositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));
		}

		public TSessionAdapter Session { get;}
		public IRepositoryFactory RepositoryFactory { get; }

		ISessionAdapter IRepositorySession.Session => this.Session;

		public void Dispose()
		{
			this.Session.Dispose();
		}
	}
}

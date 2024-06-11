using System;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public interface IRepositorySessionFactory<TSessionAdapter>
		where TSessionAdapter : class, ISessionAdapter
	{
		RepositorySession<TSessionAdapter> CreateSession(CreateSessionOptions options = null);
	}

	public class DefaultRepositorySessionFactory<TSessionAdapter> : IRepositorySessionFactory<TSessionAdapter>
		where TSessionAdapter : class, ISessionAdapter
	{
		private readonly ISessionResolver<TSessionAdapter> _resolver;
		private readonly IRepositoryFactory _repositoryFactory;

		public DefaultRepositorySessionFactory(
			ISessionResolver<TSessionAdapter> resolver,
			IRepositoryFactory repositoryFactory)
		{
			this._resolver = resolver
				?? throw new ArgumentNullException(nameof(resolver));

			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));
		}

		public RepositorySession<TSessionAdapter> CreateSession(CreateSessionOptions options = null)
		{
			return new RepositorySession<TSessionAdapter>(
				this._resolver.CreateSession(options),
				this._repositoryFactory);
		}
	}
}

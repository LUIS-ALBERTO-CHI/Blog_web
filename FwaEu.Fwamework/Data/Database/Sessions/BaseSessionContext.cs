using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public abstract class BaseSessionContext<TSessionAdapter> : IDisposable
		where TSessionAdapter : class, ISessionAdapter
	{
		public BaseSessionContext(
			IRepositorySessionFactory<TSessionAdapter> repositorySessionFactory,
			IServiceProvider serviceProvider)
		{
			_ = repositorySessionFactory
				?? throw new ArgumentNullException(nameof(repositorySessionFactory));

			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this._repositorySession = new Lazy<RepositorySession<TSessionAdapter>>(
				() => repositorySessionFactory.CreateSession(this.GetCreateSessionOptions()));
		}

		public IServiceProvider ServiceProvider { get; }

		private readonly Lazy<RepositorySession<TSessionAdapter>> _repositorySession;
		public RepositorySession<TSessionAdapter> RepositorySession
		{
			get { return this._repositorySession.Value; }
		}

		protected virtual CreateSessionOptions GetCreateSessionOptions()
		{
			return null; // NOTE: Default connection
		}

		public virtual void Dispose()
		{
			if (this._repositorySession.IsValueCreated)
			{
				this._repositorySession.Value.Dispose();
			}
		}
	}
}

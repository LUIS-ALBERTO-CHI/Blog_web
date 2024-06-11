using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	/// <summary>
	/// Resolves the ISessionAdapterFactory type from a session adapter type.
	/// </summary>
	public interface ISessionResolver<TSessionAdapter>
		where TSessionAdapter : class, ISessionAdapter
	{
		TSessionAdapter CreateSession(CreateSessionOptions options = null);
	}

	public abstract class SessionResolverBase<TSessionAdapter, TFactory> : ISessionResolver<TSessionAdapter>
		where TSessionAdapter : class, ISessionAdapter
		where TFactory : class, ISessionAdapterFactory
	{
		protected TFactory Factory { get; }

		protected SessionResolverBase(TFactory factory)
		{
			this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public abstract TSessionAdapter CreateSession(CreateSessionOptions options = null);
	}

	public class StatelessSessionResolver<TSessionAdapter, TFactory> : SessionResolverBase<TSessionAdapter, TFactory>
		where TSessionAdapter : class, ISessionAdapter
		where TFactory : class, ISessionAdapterFactory
	{
		public StatelessSessionResolver(TFactory factory) : base(factory)
		{
		}

		public override TSessionAdapter CreateSession(CreateSessionOptions options = null)
		{
			return (TSessionAdapter)this.Factory.CreateStatelessSession(options);
		}
	}

	public class StatefulSessionResolver<TSessionAdapter, TFactory> : SessionResolverBase<TSessionAdapter, TFactory>
		where TSessionAdapter : class, ISessionAdapter
		where TFactory : class, ISessionAdapterFactory
	{
		public StatefulSessionResolver(TFactory factory) : base(factory)
		{
		}

		public override TSessionAdapter CreateSession(CreateSessionOptions options = null)
		{
			return (TSessionAdapter)this.Factory.CreateStatefulSession(options);
		}
	}
}

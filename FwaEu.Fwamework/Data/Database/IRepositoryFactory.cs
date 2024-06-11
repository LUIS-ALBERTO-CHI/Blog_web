using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IRepositoryFactory
	{
		TRepository Create<TRepository>(ISessionAdapter session);
		IRepository CreateByEntityType(Type entityType, ISessionAdapter session);
	}

	public class RepositoryFactory : IRepositoryFactory
	{
		public RepositoryFactory(IRepositoryRegister repositoryRegister, IServiceProvider serviceProvider)
		{
			this._repositoryRegister = repositoryRegister;
			this._serviceProvider = serviceProvider;
		}

		private readonly IRepositoryRegister _repositoryRegister;
		private readonly IServiceProvider _serviceProvider;

		private IRepository Create(Type repositoryImplementationType, ISessionAdapter session)
		{
			var repository = (IRepository)Activator.CreateInstance(repositoryImplementationType);
			repository.Configure(session, this._serviceProvider);
			return repository;
		}

		public TRepository Create<TRepository>(ISessionAdapter session)
		{
			return (TRepository)this.Create(this._repositoryRegister
				.GetByRepositoryType(typeof(TRepository)), session);
		}

		public IRepository CreateByEntityType(Type entityType, ISessionAdapter session)
		{
			return this.Create(this._repositoryRegister
				.GetByEntityType(entityType), session);
		}
	}
}

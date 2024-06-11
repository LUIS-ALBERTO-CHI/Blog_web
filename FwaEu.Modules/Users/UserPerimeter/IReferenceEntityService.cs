using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.UserPerimeter
{
	public interface IReferenceEntityService<TEntity, TEntityId>
	{
		Task<TEntity> GetAsync(TEntityId id);
	}

	public class RepositoryReferenceEntityService<TEntity, TEntityId, TRepository> : IReferenceEntityService<TEntity, TEntityId>
		where TEntity : class
		where TRepository : IRepository<TEntity, TEntityId>
	{
		private readonly Lazy<TRepository> _repository;

		public RepositoryReferenceEntityService(IRepositoryFactory repositoryFactory, ISessionAdapter session)
		{
			_ = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
			_ = session ?? throw new ArgumentNullException(nameof(session));

			this._repository = new Lazy<TRepository>(() => repositoryFactory.Create<TRepository>(session));
		}

		public async Task<TEntity> GetAsync(TEntityId id)
		{
			return await this._repository.Value.GetAsync(id);
		}
	}
}

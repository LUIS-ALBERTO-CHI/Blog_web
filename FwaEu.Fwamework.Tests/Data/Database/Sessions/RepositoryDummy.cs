using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Tests.Data.Database.Sessions
{
	public class DummyEntity
	{

	}

	public class RepositoryDummy : IRepository<DummyEntity, int>, IRepository
	{
		public void Configure(ISessionAdapter session, IServiceProvider serviceProvider)
		{
			//NOTE: No code here, will be called during creating of repository
		}

		public DummyEntity CreateNew()
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(DummyEntity entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(object entity)
		{
			throw new NotImplementedException();
		}

		public Task<DummyEntity> GetAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<object> GetAsync(object id)
		{
			throw new NotImplementedException();
		}

		public EntityDescriptor GetEntityDescriptor()
		{
			return new EntityDescriptor(typeof(DummyEntity), typeof(int));
		}

		public Task<DummyEntity> GetNoPerimeterAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<object> GetNoPerimeterAsync(object id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<DummyEntity> Query()
		{
			throw new NotImplementedException();
		}

		public IQueryable<DummyEntity> QueryNoPerimeter()
		{
			throw new NotImplementedException();
		}

		public Task SaveOrUpdateAsync(DummyEntity entity)
		{
			throw new NotImplementedException();
		}

		public Task SaveOrUpdateAsync(object entity)
		{
			throw new NotImplementedException();
		}

		object IRepository.CreateNew()
		{
			throw new NotImplementedException();
		}

		IQueryable IRepository.Query()
		{
			throw new NotImplementedException();
		}

		IQueryable IRepository.QueryNoPerimeter()
		{
			throw new NotImplementedException();
		}
	}
}

using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	public class CarModelRepositoryDummy : IRepository<CarModel, int>
	{
		public void Configure(ISessionAdapter session, IServiceProvider serviceProvider)
		{
			throw new NotImplementedException();
		}

		public CarModel CreateNew()
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(CarModel entity)
		{
			throw new NotImplementedException();
		}

		public Task<CarModel> GetAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<CarModel> GetNoPerimeterAsync(int id)
		{
			throw new NotImplementedException();
		}

		public EntityDescriptor GetEntityDescriptor()
		{
			throw new NotImplementedException();
		}

		public IQueryable<CarModel> Query()
		{
			throw new NotImplementedException();
		}

		public IQueryable<CarModel> QueryNoPerimeter()
		{
			throw new NotImplementedException();
		}

		public Task SaveOrUpdateAsync(CarModel entity)
		{
			throw new NotImplementedException();
		}
	}
}

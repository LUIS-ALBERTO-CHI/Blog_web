using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.GenericImporter.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	public class CarModelEntityDataAccessFactoryFake : IEntityDataAccessFactory<CarModel>
	{
		private readonly EntityDataAccess<CarModel> _underlyingDataAccess;

		public CarModelEntityDataAccessFactoryFake(EntityDataAccess<CarModel> underlyingDataAccess)
		{
			this._underlyingDataAccess = underlyingDataAccess
				?? throw new ArgumentNullException(nameof(underlyingDataAccess));
		}

		public IDataAccess CreateDataAccess(ServiceStore serviceStore)
		{
			return this._underlyingDataAccess; ;
		}
	}
}

using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public interface IDataAccessFactory
	{
		IDataAccess CreateDataAccess(ServiceStore serviceStore);
	}

	public interface IDataAccessFactory<TModel> : IDataAccessFactory
	{
	}
}

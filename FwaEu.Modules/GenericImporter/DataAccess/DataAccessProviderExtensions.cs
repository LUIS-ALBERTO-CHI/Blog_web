using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public static class DataAccessProviderExtensions
	{
		public static IDataAccess<TModel> GetDataAccess<TModel>(this DataAccessProvider dataAccessProvider)
		{
			var dataAccess = dataAccessProvider.GetDataAccess(typeof(TModel));
			return (IDataAccess<TModel>)dataAccess;
		}
	}
}

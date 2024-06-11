using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors
{
	public interface IDispatchInterceptor
	{
		SqlString OnPrepareStatement(SqlString sql);
	}

	public class EmptyDispatchInterceptor : IDispatchInterceptor
	{
		public virtual SqlString OnPrepareStatement(SqlString sql)
		{
			return sql;
		}
	}
}

using FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Data.Nhibernate
{
	public class CountQueriesDispatchInterceptorMock : EmptyDispatchInterceptor
	{
		public CountQueriesDispatchInterceptorMock(Action<SqlString> onPrepareStatement)
		{
			this._onPrepareStatement = onPrepareStatement
				?? throw new ArgumentNullException(nameof(onPrepareStatement));
		}

		private Action<SqlString> _onPrepareStatement;

		public override SqlString OnPrepareStatement(SqlString sql)
		{
			this._onPrepareStatement(sql);
			return base.OnPrepareStatement(sql);
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Nhibernate.Interceptors
{
	public class CompositeDispatchInterceptor : EmptyInterceptor
	{
		public CompositeDispatchInterceptor(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this.DispatchInterceptors = new Lazy<IDispatchInterceptor[]>(
				() => this.ServiceProvider.GetServices<IDispatchInterceptor>().ToArray());
		}

		protected IServiceProvider ServiceProvider { get; }
		protected Lazy<IDispatchInterceptor[]> DispatchInterceptors { get; }

		public override SqlString OnPrepareStatement(SqlString sql)
		{
			foreach (var interceptor in this.DispatchInterceptors.Value)
			{
				sql = interceptor.OnPrepareStatement(sql);
			}

			return base.OnPrepareStatement(sql);
		}
	}
}

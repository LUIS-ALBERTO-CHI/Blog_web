using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Data.Database.Sessions
{
	public class MainSessionContext : BaseSessionContext<IStatefulSessionAdapter>
	{
		public MainSessionContext(
			IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory,
			IServiceProvider serviceProvider)
			: base(repositorySessionFactory, serviceProvider)
		{
		}
	}
}

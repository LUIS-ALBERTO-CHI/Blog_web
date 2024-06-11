using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.TestTools.InMemoryDatabase.Sqlite
{
	public static class InMemoryDataScopeExtensions
	{
		public static T CreateRepository<T>(this IDataScope dataScope)
		{
			return dataScope.ServiceProvider.GetRequiredService<IRepositoryFactory>().Create<T>(dataScope.Session);
		}
	}
}

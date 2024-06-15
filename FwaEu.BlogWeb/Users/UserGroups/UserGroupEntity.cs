using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.SimpleMasterData;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.BlogWeb.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users.UserGroups
{
	public class UserGroupEntity : SimpleMasterDataEntityBase
	{
	}

	public class UserGroupEntityClassMap : SimpleMasterDataEntityBaseClassMap<UserGroupEntity>
	{
		public UserGroupEntityClassMap() : base()
		{
		}
	}

	public class UserGroupEntityRepository : SimpleMasterDataEntityBaseRepository<UserGroupEntity>
	{
		protected override IEnumerable<IRepositoryDataFilter<UserGroupEntity, int>> CreateDataFilters(
			RepositoryDataFilterContext<UserGroupEntity, int> context)
		{
			yield return new UserGroupEntityDataFilter();
		}
	}
}

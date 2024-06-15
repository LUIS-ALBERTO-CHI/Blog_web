using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.SimpleMasterData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Entities
{
	public class FarmActivityEntity : SimpleMasterDataEntityBase
	{
	}

	public class FarmActivityEntityClassMap : SimpleMasterDataEntityBaseClassMap<FarmActivityEntity>
	{
		public FarmActivityEntityClassMap() : base()
		{
		}
	}

	public class FarmActivityEntityRepository : SimpleMasterDataEntityBaseRepository<FarmActivityEntity>
	{
	}
}

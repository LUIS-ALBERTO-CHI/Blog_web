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
	public class FarmAnimalSpeciesEntity : SimpleMasterDataEntityBase
	{

	}

	public class FarmAnimalSpeciesEntityClassMap : SimpleMasterDataEntityBaseClassMap<FarmAnimalSpeciesEntity>
	{
		public FarmAnimalSpeciesEntityClassMap() : base()
		{
		}
	}

	public class FarmAnimalSpeciesEntityRepository : SimpleMasterDataEntityBaseRepository<FarmAnimalSpeciesEntity>
	{
	}
}

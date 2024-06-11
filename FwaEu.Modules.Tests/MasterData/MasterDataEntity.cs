using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FwaEu.Modules.Tests.MasterData
{
	public class MasterDataEntity : IEntity, IUpdatedOnTracked
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class MasterDataEntityClassMap : ClassMap<MasterDataEntity>
	{
		public MasterDataEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.Name).Not.Nullable().Unique();
			Map(entity => entity.UpdatedOn).Not.Nullable();
		}
	}

	public class MasterDataEntityRepository : DefaultRepository<MasterDataEntity, int>, IQueryByIds<MasterDataEntity, int>
	{
		public IQueryable<MasterDataEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}
	}
}

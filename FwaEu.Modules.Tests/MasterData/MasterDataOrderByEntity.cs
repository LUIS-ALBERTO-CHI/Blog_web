using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FwaEu.Modules.Tests.MasterData
{
	public class MasterDataOrderByEntity : IEntity, IUpdatedOnTracked
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime UpdatedOn { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}
	public class MasterDataOrderByEntityClassMap : ClassMap<MasterDataOrderByEntity>
	{
		public MasterDataOrderByEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.FirstName).Not.Nullable();
			Map(entity => entity.LastName).Not.Nullable();
		}
	}

	public class MasterDataOrderByEntityRepository : DefaultRepository<MasterDataOrderByEntity, int>, IQueryByIds<MasterDataOrderByEntity, int>
	{
		public IQueryable<MasterDataOrderByEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}
	}

	public class MasterDataOrderByEntityMasterDataProvider : EntityMasterDataProvider<MasterDataOrderByEntity, int, MasterDataOrderByModel, MasterDataOrderByEntityRepository>
	{
		public MasterDataOrderByEntityMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<MasterDataOrderByEntity, MasterDataOrderByModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new MasterDataOrderByModel()
			{
				Id = entity.Id,
				FirstName = entity.FirstName,
				LastName = entity.LastName
			};
		}
	}

	public class MasterDataOrderByModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

	}
}

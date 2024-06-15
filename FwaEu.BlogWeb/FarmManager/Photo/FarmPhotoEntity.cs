using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.BlogWeb.FarmManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Photo
{
	public class FarmPhotoEntity : IUpdateTracked
	{
		public int Id { get; set; }

		public FarmEntity Farm { get; set; }

		public string StorageRelativePath { get; set; }
		public string OriginalFileName { get; set; }

		public DateTime UpdatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }

		public bool IsNew() => this.Id == 0;
	}

	public class FarmPhotoEntityClassMap : ClassMap<FarmPhotoEntity>
	{
		public FarmPhotoEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.Farm).Not.Nullable().Unique();
			Map(entity => entity.StorageRelativePath).Not.Nullable();
			Map(entity => entity.OriginalFileName).Not.Nullable();
			this.AddUpdateTrackedPropertiesIntoMapping();
		}
	}

	public class FarmPhotoEntityRepository : DefaultRepository<FarmPhotoEntity, int>
	{
	}
}

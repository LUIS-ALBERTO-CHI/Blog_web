using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsProvidersByEntities
{
	public class ReportEntity : ICreationAndUpdateTracked
	{
		public int Id { get; protected set; }
		public string InvariantId { get; set; }
		public bool IsActive { get; set; }
		public string Json { get; set; }
		public bool IsAsync { get; set; }
		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class ReportEntityClassMap : ClassMap<ReportEntity>
	{
		public ReportEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.InvariantId).Not.Nullable().Unique();
			Map(entity => entity.IsActive).Not.Nullable();
			Map(entity => entity.Json).Not.Nullable().Length(4001);
			Map(entity => entity.IsAsync).Not.Nullable();
			this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}

	public class ReportEntityRepository : DefaultRepository<ReportEntity, int>
	{
		public async Task<ReportEntity> FindByInvariantIdAsync(string invariantId)
		{
			return await this.Query().FirstOrDefaultAsync(entity => entity.InvariantId == invariantId);
		}
	}
}

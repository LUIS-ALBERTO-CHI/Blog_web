using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.ReportsPerimeters
{
	public class UserReportPerimeterEntity : IPerimeterEntity<string>
	{
		public int Id { get; protected set; }

		public UserEntity User { get; set; }
		public string ReportInvariantId { get; set; }

		public string GetReferenceEntityId() => this.ReportInvariantId;
		public bool IsNew() => this.Id == 0;
	}

	public class UserReportPerimeterEntityClassMap : ClassMap<UserReportPerimeterEntity>
	{
		public UserReportPerimeterEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().UniqueKey("UQ_User_ReportInvariantId");
			Map(entity => entity.ReportInvariantId).UniqueKey("UQ_User_ReportInvariantId"); // NOTE: NULL means full access to user groups
		}
	}

	public class UserReportPerimeterEntityRepository : DefaultRepository<UserReportPerimeterEntity, int>
	{
	}
}

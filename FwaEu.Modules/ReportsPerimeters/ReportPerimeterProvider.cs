using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FwaEu.Modules.ReportsPerimeters
{
	public class ReportPerimeterProviderFactory : IUserPerimeterProviderFactory
	{
		public const string ProviderKey = "Reports";
		public string Key => ProviderKey;

		public IUserPerimeterProvider Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<ReportPerimeterProvider>();
		}
	}

	public class ReportPerimeterProvider : EntityUserPerimeterProviderBase
			<UserReportPerimeterEntity, UserReportPerimeterEntityRepository, string, string>
	{
		public ReportPerimeterProvider(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		protected override UserReportPerimeterEntity CreatePerimeterEntity(string referenceEntity)
		{
			return new UserReportPerimeterEntity()
			{
				ReportInvariantId = referenceEntity,
			};
		}

		protected override IReferenceEntityService<string, string> CreateReferenceEntityService(ISessionAdapter session)
		{
			return new ReportReferenceEntityService();
		}

		protected override Expression<Func<UserReportPerimeterEntity, string>> SelectReferenceEntityId()
		{
			return rp => rp.ReportInvariantId;
		}

		protected override Expression<Func<UserReportPerimeterEntity, bool>> WhereContainsReferenceEntities(string[] ids)
		{
			return rp => ids.Contains(rp.ReportInvariantId);
		}

		protected override Expression<Func<UserReportPerimeterEntity, bool>> WhereFullAccess()
		{
			return rp => rp.ReportInvariantId == null;
		}

		protected override Expression<Func<UserReportPerimeterEntity, bool>> WhereNotFullAccess()
		{
			return rp => rp.ReportInvariantId != null;
		}
	}
}

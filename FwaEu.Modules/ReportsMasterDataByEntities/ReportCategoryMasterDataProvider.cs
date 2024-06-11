using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsMasterDataByEntities
{
	public class ReportCategoryMasterDataProvider : EntityMasterDataProvider<ReportCategoryEntity, int,
		ReportCategoryMasterDataModel, ReportCategoryEntityRepository, string>
	{
		public ReportCategoryMasterDataProvider(
			MainSessionContext sessionContext,
			ICulturesService culturesService)
				: base(sessionContext, culturesService)
		{
		}

		protected override Expression<Func<ReportCategoryEntity, ReportCategoryMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
		{
			return entity => new ReportCategoryMasterDataModel(entity.InvariantId,
				(string)entity.Name[userCulture.TwoLetterISOLanguageName]
				?? (string)entity.Name[defaultCulture.TwoLetterISOLanguageName],
				(string)entity.Description[userCulture.TwoLetterISOLanguageName]
				?? (string)entity.Description[defaultCulture.TwoLetterISOLanguageName]);
		}
	}

	public class ReportCategoryMasterDataModel
	{
		public ReportCategoryMasterDataModel(string invariantId,
			string name, string description)
		{
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Description = description;
		}

		public string InvariantId { get; }
		public string Name { get; }
		public string Description { get; }
	}
}

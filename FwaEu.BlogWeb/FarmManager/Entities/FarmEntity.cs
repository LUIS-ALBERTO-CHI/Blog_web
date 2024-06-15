using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;


namespace FwaEu.BlogWeb.FarmManager.Entities
{
	public enum FarmCategorySize
	{
		Small = 0,
		Medium = 10,
		Large = 20
	}

	public class FarmEntity : ICreationAndUpdateTracked
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public FarmPostalCodeEntity PostalCode { get; set; }
		public string Comments { get; set; }
		public FarmActivityEntity MainActivity { get; set; }
		public decimal? SellingPriceInEurosWithoutTaxes { get; set; }
		public bool RecruitEmployees { get; set; }
		public DateTime OpeningDate { get; set; }
		public DateTime? ClosingDate { get; set; }
		public FarmCategorySize? CategorySize { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity CreatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }
		public UserEntity Responsible { get; set; }

		public bool IsNew() => Id == 0;

		public override string ToString()
		{
			return Name;
		}
	}

	public class FarmEntityClassMap : ClassMap<FarmEntity>
	{
		public FarmEntityClassMap()
		{
			const string NameAndTownPostalCodeUniqueKey = "NameAndTownPostalCodeUniqueKey";
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			Map(entity => entity.Name).Not.Nullable().UniqueKey(NameAndTownPostalCodeUniqueKey);
			References(entity => entity.PostalCode).Not.Nullable().UniqueKey(NameAndTownPostalCodeUniqueKey);
			Map(entity => entity.Comments);
			References(entity => entity.MainActivity).Not.Nullable();
			Map(entity => entity.SellingPriceInEurosWithoutTaxes).Nullable();
			Map(entity => entity.RecruitEmployees).Not.Nullable();
			Map(entity => entity.OpeningDate).Not.Nullable();
			Map(entity => entity.ClosingDate).Nullable();
			Map(entity => entity.CategorySize).Nullable().CustomType<FarmCategorySize>();
			References(entity => entity.Responsible);
			this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}

	public class FarmEntityRepository : DefaultRepository<FarmEntity, int>
	{
		protected override IEnumerable<IRepositoryDataFilter<FarmEntity, int>> CreateDataFilters(
			RepositoryDataFilterContext<FarmEntity, int> context)
		{
			yield return new FarmEntityDataFilter();
		}
	}
}

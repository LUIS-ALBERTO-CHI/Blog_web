using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Text;
using System;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IEntityKeyResolver
	{
		string ResolveKey();
	}
	public interface IEntityKeyResolver<TEntity> : IEntityKeyResolver
	where TEntity : IEntity
	{
	}
	public class PluralizationEntityResolver<TEntity> : IEntityKeyResolver<TEntity>
		where TEntity : IEntity
	{
		private readonly IPluralizationService _pluralizationService;

		public PluralizationEntityResolver(IPluralizationService pluralizationService)
		{
			_pluralizationService = pluralizationService
				?? throw new ArgumentNullException(nameof(pluralizationService));
		}

		public string ResolveKey()
		{
			var name = FwaConventions.NormalizeEntityName(typeof(TEntity));
			return _pluralizationService.Pluralize(name);
		}
	}

	public class PluralizationEntityResolver 
	{
		public string ResolveKey(Type type)
		{
			string name = FwaConventions.NormalizeEntityName(type);
			PluralizeNetCorePluralizationService plural = new PluralizeNetCorePluralizationService();
			return plural.Pluralize(name);
		}
	}
}

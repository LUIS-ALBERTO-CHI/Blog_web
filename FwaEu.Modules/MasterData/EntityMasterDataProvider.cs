using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Data.Database.Tracking;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Globalization;
using FwaEu.Fwamework.Globalization;
using System.Reflection;
using NHibernate.SqlCommand;

namespace FwaEu.Modules.MasterData
{
	public interface IEntityMasterDataProvider<TEntity>: IMasterDataProvider
		where TEntity : class, IUpdatedOnTracked
	{
	}
	public abstract class EntityMasterDataProvider<TEntity, TIdentifier, TModel, TRepository>
		: EntityMasterDataProvider<TEntity, TIdentifier, TModel, TRepository, TIdentifier>
		where TEntity : class, IUpdatedOnTracked
		where TModel : class
		where TRepository : IRepository<TEntity, TIdentifier>, IQueryByIds<TEntity, TIdentifier>
	{
		protected EntityMasterDataProvider(
			BaseSessionContext<IStatefulSessionAdapter> sessionContext,
			ICulturesService culturesService)
			: base(sessionContext, culturesService)
		{
		}
	}

	public abstract class EntityMasterDataProvider<TEntity, TIdentifier, TModel, TRepository, ITQueryByIdIdentifier> : IEntityMasterDataProvider<TEntity>
		where TEntity : class, IUpdatedOnTracked
		where TModel : class
		where TRepository : IRepository<TEntity, TIdentifier>, IQueryByIds<TEntity, ITQueryByIdIdentifier>
	{
		protected EntityMasterDataProvider(
			BaseSessionContext<IStatefulSessionAdapter> sessionContext,
			ICulturesService culturesService)
		{
			this.SessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));

			this.CulturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));
		}

		protected BaseSessionContext<IStatefulSessionAdapter> SessionContext { get; }
		protected ICulturesService CulturesService { get; }

		public Type IdType => typeof(ITQueryByIdIdentifier);

		protected abstract Expression<Func<TEntity, TModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture);

		protected virtual Expression<Func<TEntity, bool>> CreateSearchExpression(string search,
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			throw new NotImplementedException(
				$"To use search when requesting a master-data, you have to override the {nameof(CreateSearchExpression)} method.");
		}

		protected virtual TRepository CreateRepository()
		{
			return (TRepository)this.SessionContext.RepositorySession
				.CreateByEntityType(typeof(TEntity));
		}

		protected virtual IQueryable<TEntity> GetBaseQuery()
		{
			return this.CreateRepository().Query();
		}

		protected virtual IQueryable<TEntity> GetBaseQueryByIds(ITQueryByIdIdentifier[] ids)
		{
			return this.CreateRepository().QueryByIds(ids);
		}

		protected IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, MasterDataPaginationParameters pagination)
		{
			return query
				.Skip(pagination.Skip)
				.Take(pagination.Take);
		}

		protected virtual IQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query, OrderByParameter[] orderBy,
			CultureInfo userCulture, CultureInfo defaultCulture)
		{
			// NOTE: According to source code https://github.com/StefH/System.Linq.Dynamic.Core/blob/master/src/System.Linq.Dynamic.Core/DynamicQueryableExtensions.cs

			var localizableStringProperties = typeof(TEntity).GetProperties()
				.Where(p => orderBy.Any(ob => ob.PropertyName == p.Name) && p.GetCustomAttribute<LocalizableStringAttribute>() != null)
				.Select(p => p.Name)
				.ToArray();

			if (localizableStringProperties.Any())
			{
				var targetCultureCodes = new[] { userCulture.TwoLetterISOLanguageName, defaultCulture.TwoLetterISOLanguageName };
				foreach (var localizableStringPropertyName in localizableStringProperties)
				{
					var mappedCultureCodes = SessionContext.RepositorySession.Session.GetMappedCultureCodes<TEntity>(localizableStringPropertyName);
					var propertyName = string.Join(" ?? ", targetCultureCodes.Where(cc => mappedCultureCodes.Contains(cc))
						.Select(cultureCode => $"{localizableStringPropertyName}[\"{cultureCode}\"]")
						.ToArray());

					var orderByPropertyIndex = orderBy.ToList().FindIndex(ob => ob.PropertyName == localizableStringPropertyName);
					orderBy[orderByPropertyIndex] = new OrderByParameter(propertyName, orderBy[orderByPropertyIndex].Ascending);
				}
			}

			var ordering = String.Join(", ",
				orderBy.Select(ob => ob.PropertyName + " " + (ob.Ascending ? "ASC" : "DESC")));

			return query.OrderBy(ordering);
		}

		private IQueryable<TEntity> ApplyQueryAlterations(IQueryable<TEntity> query, MasterDataProviderGetModelsParameters parameters)
		{
			if (parameters.Search != null)
			{
				query = query.Where(this.CreateSearchExpression(parameters.Search, parameters.Culture, this.CulturesService.DefaultCulture));
			}

			if (parameters.OrderBy != null)
			{
				query = this.ApplyOrderBy(query, parameters.OrderBy, parameters.Culture, this.CulturesService.DefaultCulture);
			}

			if (parameters.Pagination != null)
			{
				query = this.ApplyPagination(query, parameters.Pagination);
			}

			return query;
		}

		public virtual async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
		{
			var query = this.GetBaseQuery();

			var info = await query.GroupBy(e => 1)
				.Select(g => new MasterDataChangesInfo(
					g.Count() > 0 ? (DateTime?)g.Max(e => e.UpdatedOn) : default(DateTime?),
					g.Count()))
				.SingleOrDefaultAsync();

			return info;
		}

		public async Task<IEnumerable<TModel>> GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
		{
			var query = this.GetBaseQuery();
			query = this.ApplyQueryAlterations(query, parameters);

			return await query
				.Select(this.CreateSelectExpression(parameters.Culture, this.CulturesService.DefaultCulture))
				.ToListAsync();
		}

		protected virtual async Task<IEnumerable<TModel>> GetModelsByIdsAsync(ITQueryByIdIdentifier[] ids, CultureInfo culture)
		{
			var query = this.GetBaseQueryByIds(ids);

			return await query
				.Select(this.CreateSelectExpression(culture, this.CulturesService.DefaultCulture))
				.ToListAsync();
		}

		public async Task<IEnumerable<TModel>> GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
		{
			return await this.GetModelsByIdsAsync(
				parameters.Ids.Cast<ITQueryByIdIdentifier>().ToArray(), parameters.Culture);
		}

		async Task<IEnumerable> IMasterDataProvider.GetModelsByIdsAsync(MasterDataProviderGetModelsByIdsParameters parameters)
		{
			return await this.GetModelsByIdsAsync(parameters);
		}

		async Task<IEnumerable> IMasterDataProvider.GetModelsAsync(MasterDataProviderGetModelsParameters parameters)
		{
			return await this.GetModelsAsync(parameters);
		}
	}
}

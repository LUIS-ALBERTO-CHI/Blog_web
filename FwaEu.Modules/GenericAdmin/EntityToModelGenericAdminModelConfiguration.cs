using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using NHibernate.Linq;
using NHibernate.Exceptions;
using FwaEu.Modules.Data.Database;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Reflection;

namespace FwaEu.Modules.GenericAdmin
{
	public abstract class EntityToModelGenericAdminModelConfiguration<TEntity, TIdentifier, TModel, TSessionContext>
		: ModelAttributeGenericAdminModelConfiguration<TModel>
		where TEntity : class, IEntity, new()
		where TModel : new()
		where TSessionContext : BaseSessionContext<IStatefulSessionAdapter>
	{
		private Lazy<TSessionContext> _sessionContext;
		protected ICulturesService CulturesService { get; }

		protected EntityToModelGenericAdminModelConfiguration(
			IServiceProvider serviceProvider,
			ICulturesService culturesService)
			: base(serviceProvider)
		{
			this._sessionContext = new Lazy<TSessionContext>(
				() => serviceProvider.GetRequiredService<TSessionContext>());

			this.CulturesService = culturesService;
		}

		protected RepositorySession<IStatefulSessionAdapter> RepositorySession => this._sessionContext.Value.RepositorySession;

		protected virtual IRepository<TEntity, TIdentifier> GetRepository()
		{
			return (IRepository<TEntity, TIdentifier>)this.RepositorySession.CreateByEntityType(typeof(TEntity));
		}

		protected abstract Expression<Func<TEntity, TModel>> GetSelectExpression();

		protected virtual IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, GenericAdminPaginationParameters pagination)
		{
			if (pagination.Skip != null)
				query = query.Skip(pagination.Skip.Value);

			if (pagination.Take != null)
				query = query.Take(pagination.Take.Value);

			return query;
		}

		private void MapDefaultModelPropertiesToEntityProperties(GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> mapper)
		{
			var modelProperties = TypeDescriptor.GetProperties(typeof(TModel));
			var entityProperties = TypeDescriptor.GetProperties(typeof(TEntity));

			foreach (var modelProperty in modelProperties.Cast<PropertyDescriptor>())
			{
				if (null == mapper.GetColumnExpression(modelProperty.Name, null))
				{
					var entityProperty = entityProperties.Find(modelProperty.Name, false);
					if (null != entityProperty)
					{
						var lambdaParameterExpression = Expression.Parameter(typeof(TEntity));
						var propertyExpression = Expression.Property(lambdaParameterExpression, entityProperty.Name) as Expression;
						var lambdaExpression = Expression.Lambda(propertyExpression, lambdaParameterExpression);

						mapper.MapModelPropertyToEntityProperty(entityProperty.Name, lambdaExpression);
					}
				}
			}
		}

		protected virtual void MapSortModelPropertiesToEntityProperties(
			GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> mapper,
			CultureInfo userCulture,
			CultureInfo defaultCulture)
		{
			MapDefaultModelPropertiesToEntityProperties(mapper);
		}

		protected virtual void MapFilterModelPropertiesToEntityProperties(
			GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> mapper,
			CultureInfo userCulture,
			CultureInfo defaultCulture)
		{
			MapDefaultModelPropertiesToEntityProperties(mapper);
		}

		protected GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> BuildSortColumnMapper(CultureInfo userCulture)
		{
			var mapper = new GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel>();
			MapSortModelPropertiesToEntityProperties(mapper, userCulture, CulturesService.DefaultCulture);
			return mapper;
		}

		protected GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> BuildFilterColumnMapper(CultureInfo userCulture)
		{
			var mapper = new GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel>();
			MapFilterModelPropertiesToEntityProperties(mapper, userCulture, CulturesService.DefaultCulture);
			return mapper;
		}

		private Expression<Func<TEntity, bool>> BuilderFilterValueExpression(
			GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> mapper,
			GenericAdminFilterCondition filterCondition)
		{
			var columnDescriptor = new GenericAdminColumnDescriptor(filterCondition.ColumnName);
			var selectorLambdaExpression = mapper.GetColumnExpression(columnDescriptor.ColumnName, columnDescriptor.Locale) as LambdaExpression;
			var selectorExpression = selectorLambdaExpression.Body;
			var lambdaParameter = selectorLambdaExpression.Parameters;
			var constantExpression = Expression.Constant(filterCondition.Value) as Expression;
			if (filterCondition.Value.GetType() != selectorLambdaExpression.ReturnType)
				constantExpression = Expression.Convert(constantExpression, selectorLambdaExpression.ReturnType);

			switch (filterCondition.Mode)
			{
				case GenericAdminFilterMode.Equals        : return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(selectorExpression, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.NotEquals     : return Expression.Lambda<Func<TEntity, bool>>(Expression.NotEqual(selectorExpression, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.GreaterThan   : return Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThan(selectorExpression, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.LessThan      : return Expression.Lambda<Func<TEntity, bool>>(Expression.LessThan(selectorExpression, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.GreaterOrEqual: return Expression.Lambda<Func<TEntity, bool>>(Expression.GreaterThanOrEqual(selectorExpression, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.LessOrEqual   : return Expression.Lambda<Func<TEntity, bool>>(Expression.LessThanOrEqual(selectorExpression, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.Contains      : return Expression.Lambda<Func<TEntity, bool>>(Expression.Call(selectorExpression, "Contains", null, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.NotContains   : return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(Expression.Call(selectorExpression, "Contains", null, constantExpression)), lambdaParameter);
				case GenericAdminFilterMode.StartsWith    : return Expression.Lambda<Func<TEntity, bool>>(Expression.Call(selectorExpression, "StartsWith", null, constantExpression), lambdaParameter);
				case GenericAdminFilterMode.EndsWith      : return Expression.Lambda<Func<TEntity, bool>>(Expression.Call(selectorExpression, "EndsWith", null, constantExpression), lambdaParameter);
			}

			throw new InvalidOperationException($"Mode '{filterCondition.Mode}' not allowed on filter condition.");
		}

		protected Expression<Func<TEntity, bool>> BuildFilterExpression(
			GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> mapper,
			GenericAdminFilterConditionContainer conditionContainer)
		{
			if (conditionContainer.IsValue)
				return BuilderFilterValueExpression(mapper, conditionContainer.Condition);

			var filterExpression = default(Expression<Func<TEntity, bool>>);
			var conditionMode = GenericAdminFilterMode.None;
			foreach (var containerItem in conditionContainer.Container)
			{
				if (filterExpression == null)
				{
					filterExpression = BuildFilterExpression(mapper, containerItem.Container);
				}
				else
				{
					switch (conditionMode)
					{
						case GenericAdminFilterMode.And : filterExpression = filterExpression.And(BuildFilterExpression(mapper, containerItem.Container)); break;
						case GenericAdminFilterMode.Or  : filterExpression = filterExpression.Or(BuildFilterExpression(mapper, containerItem.Container)); break;
						case GenericAdminFilterMode.None: break;
						default: throw new InvalidOperationException($"Unhandled conditionnal filter mode '{conditionMode}'");
					}
				}
				conditionMode = containerItem.ContainerMode;
			}

			return filterExpression;
		}

		protected virtual IQueryable<TEntity> ApplyPaginationFilter(IQueryable<TEntity> query, CultureInfo userCulture, GenericAdminFilterParameters filter)
		{
			return query.Where(BuildFilterExpression(BuildFilterColumnMapper(userCulture), filter.Filters));
		}

		protected virtual IQueryable<TEntity> ApplyPaginationSort(IQueryable<TEntity> query, CultureInfo userCulture, GenericAdminSortParameters sort)
		{
			var enumerator = sort.Parameters.GetEnumerator();
			enumerator.MoveNext();

			var mapper = BuildSortColumnMapper(userCulture);

			var parameter = enumerator.Current;
			var columnDescriptor = new GenericAdminColumnDescriptor(parameter.ColumnName);
			var propertyExpression = mapper.GetColumnExpression(columnDescriptor.ColumnName, columnDescriptor.Locale);

			var orderedQuery = parameter.Ascending
				? query.OrderBy(propertyExpression)
				: query.OrderByDescending(propertyExpression);
			
			while (enumerator.MoveNext())
			{
				parameter = enumerator.Current;
				columnDescriptor = new GenericAdminColumnDescriptor(parameter.ColumnName);
				propertyExpression = mapper.GetColumnExpression(columnDescriptor.ColumnName, columnDescriptor.Locale);
			
				orderedQuery = parameter.Ascending
					? orderedQuery.ThenBy(propertyExpression)
					: orderedQuery.ThenByDescending(propertyExpression);
			}
			
			return orderedQuery;
		}

		protected virtual IQueryable<TEntity> Query()
		{
			return this.GetRepository().QueryNoPerimeter(); //NOTE: Perimeter discussion https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4508
		}

		private async Task<List<TModel>> LoadEntitiesAsync(GenericAdminGetModelsParameters options)
		{
			var query = this.Query();

			if (options?.Filter != null)
				query = ApplyPaginationFilter(query, options.UserCulture, options.Filter);

			if (options?.Sort != null)
				query = ApplyPaginationSort(query, options.UserCulture, options.Sort);

			if (options?.Pagination != null)
				query = ApplyPagination(query, options.Pagination);

			return await query.Select(this.GetSelectExpression())
				.ToListAsync();
		}

		private async Task<int?> GetModelsCountAsync(GenericAdminGetModelsParameters options)
		{
			if (options?.Pagination?.RequireTotalCount != true)
				return null;

			var query = this.Query();

			if (options?.Filter != null)
				query = ApplyPaginationFilter(query, options.UserCulture, options.Filter);

			return await query.CountAsync();
		}

		public override async Task<LoadDataResult<TModel>> GetModelsAsync(GenericAdminGetModelsParameters options)
		{
			return new LoadDataResult<TModel>(
				new ListDataSource<TModel>(
					await this.LoadEntitiesAsync(options),
					await this.GetModelsCountAsync(options))
				);
		}

		protected abstract void SetIdToModel(TModel model, TEntity entity);
		protected abstract Task<TEntity> GetEntityAsync(TModel model);
		protected abstract Task FillEntityAsync(TEntity entity, TModel model);

		private async Task<TEntity> GetEntityOrFailWhenNotFoundAsync(TModel model)
		{
			var entity = await this.GetEntityAsync(model);

			if (entity == null)
			{
				throw new NotSupportedException("Entity not found.");
			}

			return entity;
		}

		private async Task<TEntity> GetEntityOrNewOrFailWhenNotFoundAsync(TModel model)
		{
			return this.IsNew(model) ? new TEntity()
				: await this.GetEntityOrFailWhenNotFoundAsync(model);
		}

		protected override async Task<TModel> SaveModelAsync(TModel model)
		{
			var entity = await this.GetEntityOrNewOrFailWhenNotFoundAsync(model);

			await this.FillEntityAsync(entity, model);

			await this.GetRepository().SaveOrUpdateAsync(entity);
			await this.RepositorySession.Session.FlushAsync();
			this.SetIdToModel(model, entity);
			return this.GetSelectExpression().Compile()(entity);
		}

		public override async Task<SaveResult> SaveAsync(IEnumerable<TModel> models)
		{
			var session = this.RepositorySession.Session;
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var saveResult = default(SaveResult);
					try
					{
						saveResult = await base.SaveAsync(models);

						// NOTE: FlushAsync done in SaveModelAsync
						await transaction.CommitAsync();
					}
					catch (GenericADOException e)
					{
						DatabaseExceptionHelper.CheckForDbConstraints(e);
						throw;
					}
					return saveResult;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		protected override async Task<SimpleDeleteModelResult> DeleteModelAsync(TModel model)
		{
			var entity = await this.GetEntityOrNewOrFailWhenNotFoundAsync(model);
			await this.GetRepository().DeleteAsync(entity);
			return new SimpleDeleteModelResult();
		}

		public override async Task<DeleteResult> DeleteAsync(IEnumerable<TModel> models, PropertyDescriptor[] keyProperties)
		{
			var session = this.RepositorySession.Session;
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var deleteResult = default(DeleteResult);

					try
					{
						deleteResult = await base.DeleteAsync(models, keyProperties);

						await session.FlushAsync();
						await transaction.CommitAsync();
					}
					catch (GenericADOException e)
					{
						DatabaseExceptionHelper.CheckForDbConstraints(e);
						throw;
					}
					return deleteResult;
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}
		protected override Property CreateProperty(PropertyDescriptor propertyDescriptor)
		{
			var localizableStringAttributte = propertyDescriptor.Attributes.OfType<LocalizableStringCustomTypeAttribute>().FirstOrDefault();
			if (localizableStringAttributte != null)
			{
				var mappedCultureCodes = RepositorySession.Session.GetMappedCultureCodes<TEntity>(propertyDescriptor.Name);
				localizableStringAttributte.SupportedCultureCodes = mappedCultureCodes;
			}
			return base.CreateProperty(propertyDescriptor);
		}



	}

}
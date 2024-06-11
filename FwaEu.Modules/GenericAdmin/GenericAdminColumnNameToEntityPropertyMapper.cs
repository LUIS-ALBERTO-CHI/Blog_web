using FluentNHibernate.Utils;
using FwaEu.Fwamework.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FwaEu.Modules.GenericAdmin
{
	public class EntityPropertyMapping<TEntity>
	{
		public LambdaExpression EntitySelectorExpression { get; }

		public EntityPropertyMapping(LambdaExpression typedEntitySelectorExpression)
		{
			EntitySelectorExpression = typedEntitySelectorExpression;
		}
	}

	public class LocalizableEntityPropertyMapping<TEntity>
	{
		private LambdaExpression EntitySelectorExpression { get; }

		public LocalizableEntityPropertyMapping(LambdaExpression typedEntitySelectorExpression)
		{
			EntitySelectorExpression = typedEntitySelectorExpression;
		}

		public LambdaExpression GetEntitySelectorExpression(string locale)
		{
			return ExpressionHelper.ResolveExpressionParameter(EntitySelectorExpression, EntitySelectorExpression.Parameters[1], locale);
		}
	}

	public class GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel>
	{
		private Dictionary<string, EntityPropertyMapping<TEntity>> ColumnNameToEntityPropertyMapping = new();
		private Dictionary<string, LocalizableEntityPropertyMapping<TEntity>> ColumnNameToLocalizableEntityPropertyMapping = new();

		public LambdaExpression GetColumnExpression(string columnName, string locale)
		{
			if (string.IsNullOrWhiteSpace(locale))
			{
				ColumnNameToEntityPropertyMapping.TryGetValue(columnName, out var entityPropertyMapping);
				return entityPropertyMapping?.EntitySelectorExpression;
			}
			
			ColumnNameToLocalizableEntityPropertyMapping.TryGetValue(columnName, out var entityPropertyMapping1);

			return entityPropertyMapping1?.GetEntitySelectorExpression(locale);
		}

		public GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> MapModelPropertyToEntityProperty(string columnName, LambdaExpression entitySelectorExpression)
		{
			ColumnNameToEntityPropertyMapping[columnName] = new EntityPropertyMapping<TEntity>(entitySelectorExpression);
			return this;
		}

		public GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> MapModelPropertyToEntityProperty<TEntityProperty>(string columnName, Expression<Func<TEntity, TEntityProperty>> entitySelectorExpression)
		{
			ColumnNameToEntityPropertyMapping[columnName] = new EntityPropertyMapping<TEntity>(entitySelectorExpression);
			return this;
		}

		public GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> MapModelPropertyToLocalizableEntityProperty<TEntityProperty>(string columnName, Expression<Func<TEntity, string, TEntityProperty>> entityLocalizableSelectorExpression)
		{
			ColumnNameToLocalizableEntityPropertyMapping[columnName] = new LocalizableEntityPropertyMapping<TEntity>(entityLocalizableSelectorExpression);
			return this;
		}

		public GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> MapModelPropertyToEntityProperty<TModelProperty, TEntityProperty>(
			Expression<Func<TModel, TModelProperty>> modelSelectorExpression,
			Expression<Func<TEntity, TEntityProperty>> entitySelectorExpression)
		{
			MapModelPropertyToEntityProperty(modelSelectorExpression.ToMember().Name, entitySelectorExpression);
			return this;
		}

		public GenericAdminColumnNameToEntityPropertyMapper<TEntity, TModel> MapModelPropertyToLocalizableEntityProperty<TModelProperty, TEntityProperty>(
			Expression<Func<TModel, TModelProperty>> modelSelectorExpression,
			Expression<Func<TEntity, string, TEntityProperty>> entitySelectorExpression)
		{
			MapModelPropertyToLocalizableEntityProperty(modelSelectorExpression.ToMember().Name, entitySelectorExpression);
			return this;
		}
	}
}

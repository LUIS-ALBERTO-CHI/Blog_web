using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate.Tracking;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Temporal;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.GenericImporter.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public class KeyPropertyCache
	{
		public KeyPropertyCache(string name, Type type, KeyPropertyCache[] subProperties)
		{
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
			this.Type = type ?? throw new ArgumentNullException(nameof(type));

			this.SubProperties = subProperties;
		}

		public string Name { get; }
		public Type Type { get; }
		public KeyPropertyCache[] SubProperties { get; }
	}
	/// <summary>
	/// Entity key properties model in order to use ServiceStore.GetOrAdd
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	internal class EntityKeyPropertiesCache<TEntity> where TEntity : class, IEntity, new()
	{
		public KeyPropertyCache[] Properties { get; }

		public EntityKeyPropertiesCache(KeyPropertyCache[] properties)
		{
			Properties = properties ?? throw new ArgumentNullException(nameof(properties));
		}
	}


	public class EntityImporter<TEntity> : ModelImporter<TEntity, IEntityImporterEventListener>
		where TEntity : class, IEntity, new()
	{
		protected KeyPropertyCache[] LoadSubProperties(Type propertyType, ModelPropertyDescriptor key)
		{
			if (key.SearchOn == null)
			{
				return null;
			}

			var childProperties = TypeDescriptor.GetProperties(propertyType)
				.Cast<PropertyDescriptor>()
				.ToArray();

			return childProperties.Join(key.SearchOn, cp => cp.Name, so => so,
				(cp, so) => new KeyPropertyCache(cp.Name, cp.PropertyType, null))
				.ToArray();
		}

		public EntityImporter(IServiceProvider serviceProvider, IModelBinder<TEntity> modelBinder)
			: base(serviceProvider, modelBinder)
		{

		}

		protected KeyPropertyCache[] GetKeysCache(ServiceStore serviceStore, ModelPropertyDescriptor[] keys)
		{
			var entityKeyPropertiesCache = serviceStore.GetOrAdd(() =>
			{
				var properties = TypeDescriptor.GetProperties(typeof(TEntity))
					.Cast<PropertyDescriptor>()
					.ToArray();
				return new EntityKeyPropertiesCache<TEntity>(properties.Join(keys, pd => pd.Name, key => key.Name,
					(pd, key) => new KeyPropertyCache(pd.Name, pd.PropertyType,
						LoadSubProperties(pd.PropertyType, key)))
					.ToArray());
			});
			return entityKeyPropertiesCache.Properties;
		}

		protected override async Task SaveModelAsync(
			TEntity model, ModelImporterContext context)
		{
			var provider = this.GetDataAccessProvider(context.ServiceStore);
			var dataAccess = provider.GetDataAccess<TEntity>();

			await dataAccess.SaveOrUpdateAsync(model);
		}

		protected override async Task<ModelLoadResult<TEntity>> LoadModelAsync(
			ModelPropertyDescriptor[] keys, DataRow row, ModelImporterContext context)
		{
			var keysCache = this.GetKeysCache(context.ServiceStore, keys);
			var provider = this.GetDataAccessProvider(context.ServiceStore);
			var dataAccess = provider.GetDataAccess<TEntity>();

			var currentUser = ServiceProvider.GetService<ICurrentUserService>().User?.Entity;
			var now = ServiceProvider.GetService<ICurrentDateTime>().Now;
			var nameOfCreatedBy = nameof(ICreatedByTracked.CreatedBy);

			bool isCreatedByKey = keysCache.Any(x => x.Name == nameOfCreatedBy);
			bool createdByIsPresent = row.ValuesByPropertyName.ContainsKey(nameOfCreatedBy);
			bool isCreatedByFilled = createdByIsPresent && row.ValuesByPropertyName[nameOfCreatedBy] != null;

			if (!isCreatedByFilled && createdByIsPresent)
				row.ValuesByPropertyName[nameOfCreatedBy] = new[] { currentUser.Identity };

			PropertyValueSet CreatePropertyValueSet(KeyPropertyCache keyProperty)
			{
				var keyPropertyValue = row.ValuesByPropertyName[keyProperty.Name];
				return new PropertyValueSet(keyProperty.Name,
					keyProperty.SubProperties == null
					? new[] { new PropertyValue(null, keyProperty.Type, keyPropertyValue) }
					: keyProperty.SubProperties.Select((sp, i) => new PropertyValue(sp.Name, sp.Type,
						keyPropertyValue != null ? ((object[])keyPropertyValue)[i] : null)).ToArray());
			}
			var keyValues = keysCache.Select(CreatePropertyValueSet).ToArray();

			var entity = (await dataAccess.FindAsync(keyValues)) ?? new TEntity();

			// NOTE: Add new policies to insert data when CreatedBy is existed 
			if (entity.IsNew())
			{
				// NOTE: Throw exception when CreatedBy is a key and his value not found 
				if (isCreatedByKey && isCreatedByFilled)
				{
					var keyPropertyNames = String.Join(", ", keyValues.Select(a => a.PropertyName).ToArray());
					var keyPropertyValues = String.Join(", ", keyValues.Select(a => String.Join(", ", a.Data.Select(d => d.Value))).ToArray());
					throw new InvalidOperationException(String.Format("Field with key(s): {0} values: {1} not inserted.", keyPropertyNames, keyPropertyValues));
				}

				if (entity is ICreatedByTracked cb)
				{
					cb.CreatedBy = currentUser;
				}
				if (entity is ICreatedOnTracked co)
				{
					co.CreatedOn = now;
				}
			}

			if (entity is IUpdatedByTracked ub)
			{
				ub.UpdatedBy = currentUser;
			}
			if (entity is IUpdatedOnTracked uo)
			{
				uo.UpdatedOn = now;
			}

			return new ModelLoadResult<TEntity>(entity, entity.IsNew());
		}

		protected virtual DataAccessProvider CreateDataAccessProvider(ServiceStore serviceStore)
		{
			return new DataAccessProvider(serviceStore, this.ServiceProvider);
		}

		protected DataAccessProvider GetDataAccessProvider(ServiceStore serviceStore)
		{
			return serviceStore.GetOrAdd(() => this.CreateDataAccessProvider(serviceStore));
		}

		protected override Task ImportAsync(DataReader reader, ModelImporterContext context, ProcessResultContext processResultContext)
		{
			using (this.ServiceProvider.GetRequiredService<CreationOrUpdateTrackingEventListener.Disabler>().Disable())
			{
				return base.ImportAsync(reader, context, processResultContext);
			}
		}
	}
}
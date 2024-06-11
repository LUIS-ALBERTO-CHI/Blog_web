using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Reflection;
using FwaEu.Fwamework.ValueConverters;
using FwaEu.Modules.GenericImporter.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public interface IModelBinder
	{
		Task BindToModelAsync(object model, DataRow dataRow, ServiceStore serviceStore);
	}

	public interface IModelBinder<TModel> : IModelBinder
	{
		Task BindToModelAsync(TModel model, DataRow dataRow, ServiceStore serviceStore);
	}

	public class DefaultModelBinder<TModel> : IModelBinder<TModel>
	{
		public DefaultModelBinder(IValueConvertService valueConvertService, IPropertyBinder propertyBinder)
		{
			this.ValueConvertService = valueConvertService
				?? throw new ArgumentNullException(nameof(valueConvertService));

			this.PropertyBinder = propertyBinder
				?? throw new ArgumentNullException(nameof(propertyBinder));
		}

		public IPropertyBinder PropertyBinder { get; }
		public IValueConvertService ValueConvertService { get; }
		public Dictionary<string, PropertyDescriptor> PropertiesByName
		{
			get { return this._propertiesByName.Value; }
		}

		private readonly Lazy<Dictionary<string, PropertyDescriptor>> _propertiesByName
			= new Lazy<Dictionary<string, PropertyDescriptor>>(
				() => TypeDescriptor.GetProperties(typeof(TModel))
				.Cast<PropertyDescriptor>()
				.ToDictionary(p => p.Name)
			);

		protected virtual async Task<object> FindSearchOnValueAsync(IEnumerable values,
			ModelPropertyDescriptor metadataProperty, PropertyDescriptor modelProperty,
			ServiceStore serviceStore)
		{
			var relatedModelType = modelProperty.PropertyType;
			var dataAccess = serviceStore.Get<DataAccessProvider>()
				.GetDataAccess(relatedModelType);

			var data = values.Cast<object>().ToArray();

			var relatedModelPropertyTypes = serviceStore.GetOrAdd<ReflectionCache>().GetPropertyTypes(relatedModelType);
			var keys = metadataProperty.SearchOn
				.Select((so, i) =>
					new PropertyValueSet(so, new[] { new PropertyValue(
						null, relatedModelPropertyTypes[so], data[i]) }))
				.ToArray();

			return (await dataAccess.FindAsync(keys))
				?? throw new NoMatchingValueException(modelProperty, keys);
		}

		public async virtual Task BindToModelAsync(TModel model, DataRow dataRow, ServiceStore serviceStore)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}

			if (dataRow == null)
			{
				throw new ArgumentNullException(nameof(dataRow));
			}

			var propertiesByName = this.PropertiesByName;
			var invariantCulture = CultureInfo.InvariantCulture;

			foreach (var metadataProperty in dataRow.MetadataProperties.Where(mp => !mp.IsInfo))
			{
				var propertyName = metadataProperty.Name;
				// NOTE: Ignoring any changing from import
				if (propertyName == nameof(ICreationAndUpdateTracked.CreatedBy) || propertyName == nameof(ICreationAndUpdateTracked.UpdatedBy) ||
					propertyName == nameof(ICreationAndUpdateTracked.UpdatedBy) || propertyName == nameof(ICreationAndUpdateTracked.UpdatedOn))
					break;

				if (!propertiesByName.ContainsKey(propertyName))
				{
					throw new KeyNotFoundException($"Property '{propertyName}' doesn't exists on model '{typeof(TModel)}'.");
				}

				var property = propertiesByName[propertyName];
				var value = dataRow.ValuesByPropertyName[propertyName];

				if (metadataProperty.SearchOn != null && value != null)
				{
					value = await this.FindSearchOnValueAsync((IEnumerable)value, metadataProperty, property, serviceStore);
				}

				var convertedValue = this.ValueConvertService.Convert(value, property.PropertyType, invariantCulture);
				this.PropertyBinder.SetValue(property, model, convertedValue);
			}
		}

		Task IModelBinder.BindToModelAsync(object model, DataRow dataRow, ServiceStore serviceStore)
		{
			return this.BindToModelAsync((TModel)model, dataRow, serviceStore);
		}
	}
	public class NoMatchingValueException : Exception
	{
		private static string GenerateMessage(PropertyDescriptor modelProperty, PropertyValueSet[] keys)
		{
			var keysAsString = String.Join(", ",
				keys.Select(k =>
				{
					var valuesAsString = k.Data.Length == 1 && k.Data[0].Name == null
						? k.Data[0].Value?.ToString()
						: "[" + String.Join(", ",
							k.Data.Select(d => $"{d.Name}={d.Value}") + "]");

					return $"{k.PropertyName} = {valuesAsString}";
				}));

			return $"Cannot found a value for property '{modelProperty.Name}', " +
				$"with keys: {keysAsString}.";
		}

		public NoMatchingValueException(PropertyDescriptor modelProperty, PropertyValueSet[] keys)
			: base(GenerateMessage(modelProperty, keys))
		{

		}

	}
}

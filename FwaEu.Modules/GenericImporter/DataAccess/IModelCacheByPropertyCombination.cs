using FwaEu.Fwamework.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter.DataAccess
{
	public interface IModelCacheByPropertyCombination<TModel>
	{
		TModel Find(PropertyValueSet[] restrictions);
		void Add(TModel model);
	}

	public class StringKeyModelCacheByPropertyCombination<TModel> : IModelCacheByPropertyCombination<TModel>
	{
		private class PropertyValueReader
		{
			public PropertyValueReader(PropertyValueSet propertyValueSet,
				PropertyValue propertyValue, Func<TModel, object> reader)
			{
				this.PropertyValueSet = propertyValueSet
					?? throw new ArgumentNullException(nameof(propertyValueSet));

				this.PropertyValue = propertyValue
					?? throw new ArgumentNullException(nameof(propertyValue));

				this._reader = reader
					?? throw new ArgumentNullException(nameof(reader));
			}

			public PropertyValueSet PropertyValueSet { get; }
			public PropertyValue PropertyValue { get; }
			private readonly Func<TModel, object> _reader;

			public object Read(TModel model)
			{
				return this._reader(model);
			}
		}

		public StringKeyModelCacheByPropertyCombination(List<TModel> models,
			PropertyValueSet[] keys, StringComparer stringComparer)
		{
			if (models == null)
			{
				throw new ArgumentNullException(nameof(models));
			}

			if (stringComparer == null)
			{
				throw new ArgumentNullException(nameof(stringComparer));
			}

			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys));
			}

			this._keyProperties = new Lazy<PropertyValueReader[]>(() =>
			{
				var propertiesByTypeThenByName = new Dictionary<Type, Dictionary<string, PropertyDescriptor>>();

				PropertyDescriptor GetPropertyDescriptor(Type type, string propertyName)
				{
					if (!propertiesByTypeThenByName.ContainsKey(type))
					{
						propertiesByTypeThenByName.Add(type,
							TypeDescriptor.GetProperties(type)
							.Cast<PropertyDescriptor>()
							.ToDictionary(pd => pd.Name));
					}

					return propertiesByTypeThenByName[type][propertyName];
				}

				var readers = new List<PropertyValueReader>(keys.Length);

				foreach (var key in keys)
				{
					var rootProperty = GetPropertyDescriptor(typeof(TModel), key.PropertyName);

					if (key.Data.Length == 1 && key.Data[0].Name == null)
					{
						readers.Add(new PropertyValueReader(key, key.Data[0],
							(model) => rootProperty.GetValue(model)));
					}
					else
					{
						foreach (var propertyValue in key.Data)
						{
							readers.Add(new PropertyValueReader(key, propertyValue,
								(model) =>
								{
									var value = rootProperty.GetValue(model);

									if (value == null)
									{
										return null;
									}

									var property = GetPropertyDescriptor(value.GetType(), propertyValue.Name);
									return property.GetValue(value);
								}));
						}
					}
				}

				keys = null;

				return readers.ToArray();
			});

			this._modelsByKeys = new Lazy<Dictionary<string, TModel>>(() =>
			{
				var dictionary = models.ToDictionary(
					m => this.BuildStoreKey(m),
					m => m, stringComparer);

				models = null;
				stringComparer = null;

				return dictionary;
			});
		}

		private Lazy<PropertyValueReader[]> _keyProperties;
		private Lazy<Dictionary<string, TModel>> _modelsByKeys;

		protected virtual string GetStoreValue(object value)
		{
			if (value == null)
			{
				return default(string);
			}

			if (value is string stringValue)
			{
				return stringValue;
			}

			if (value is IFormattable formattableValue)
			{
				var format = value.GetType().IsNumericalWithDecimalPart()
					? "0.###########################" //HACK: Because decimal can store an unpredictable number of 0 and render it during ToString()
					: default(string);

				return formattableValue.ToString(format, CultureInfo.InvariantCulture);
			}

			return value.ToString();
		}

		private string BuildStoreKey(TModel model)
		{
			var stringRestrictions = this._keyProperties.Value.Select(
				p => this.GetStoreValue(p.Read(model)));

			return BuildStoreKey(stringRestrictions);
		}

		private static string BuildStoreKey(IEnumerable<string> restrictions)
		{
			return String.Join('|', restrictions);
		}

		public TModel Find(PropertyValueSet[] restrictions)
		{
			var stringRestrictions = restrictions.SelectMany(r => r.Data)
				.Select(d => this.GetStoreValue(d.Value));

			var storeKey = BuildStoreKey(stringRestrictions);

			return this._modelsByKeys.Value.ContainsKey(storeKey)
				? this._modelsByKeys.Value[storeKey]
				: default(TModel);
		}

		public void Add(TModel model)
		{
			var storeKey = this.BuildStoreKey(model);
			if (this._modelsByKeys.Value.ContainsKey(storeKey))
			{
				this._modelsByKeys.Value[storeKey] = model;
			}
			else
			{
				this._modelsByKeys.Value.Add(storeKey, model);
			}
		}
	}
}

using FwaEu.Fwamework.ValueConverters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdmin
{
	public abstract class ModelAttributeGenericAdminModelConfiguration<TModel> : IGenericAdminModelConfiguration
		where TModel : new()
	{
		protected ModelAttributeGenericAdminModelConfiguration(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		protected IServiceProvider ServiceProvider { get; }

		public virtual Type ModelType => typeof(TModel);

		private static void EnrichPropertyFromAttributes(Property property, PropertyDescriptor propertyDescriptor)
		{
			var maxLength = propertyDescriptor.Attributes.OfType<MaxLengthAttribute>().FirstOrDefault()?.Length;
			var isRequired = propertyDescriptor.Attributes.OfType<RequiredAttribute>().Any();

			if (maxLength.HasValue)
			{
				property.ExtendedProperties.Add("MaxLength", maxLength.Value);
			}
			property.ExtendedProperties.Add("IsRequired", isRequired);

			foreach (var attribute in propertyDescriptor.Attributes.OfType<IPropertyAttribute>().ToList())
			{
				attribute.Enrich(property);
			}
		}

		protected virtual Property CreateProperty(PropertyDescriptor propertyDescriptor)
		{
			var property = new Property(propertyDescriptor.Name,
				Property.SimplifyType(propertyDescriptor.PropertyType));

			EnrichPropertyFromAttributes(property, propertyDescriptor);

			this.OnPropertyCreated(property);
			return property;
		}

		protected virtual void OnPropertyCreated(Property property)
		{
			//NOTE: Children could customize the ExtendedProperties by overrideing this method
		}

		private static IEnumerable<PropertyDescriptor> GetTypeDescriptorProperties()
		{
			return TypeDescriptor.GetProperties(typeof(TModel))
				.Cast<PropertyDescriptor>();
		}

		private IEnumerable<PropertyDescriptor> GetTypeDescriptorPropertiesWhichAreKeys()
		{
			var keys = this.GetProperties().Where(p => p.IsKey);

			return GetTypeDescriptorProperties()
				.Join(keys, dp => dp.Name, k => k.Name, (dp, k) => dp);
		}

		public IEnumerable<Property> GetProperties()
		{
			return GetTypeDescriptorProperties()
				.Select(this.CreateProperty);
		}

		public abstract Task<LoadDataResult<TModel>> GetModelsAsync(GenericAdminGetModelsParameters parameters);

		async Task<LoadDataResult> IGenericAdminModelConfiguration.GetModelsAsync(GenericAdminGetModelsParameters parameters)
		{
			return await this.GetModelsAsync(parameters);
		}

		protected abstract bool IsNew(TModel model);
		protected abstract Task<TModel> SaveModelAsync(TModel model);

		/// <summary>
		/// Only the keys propeties of the model are filled.
		/// </summary>
		protected abstract Task<SimpleDeleteModelResult> DeleteModelAsync(TModel model);

		public virtual async Task<SaveResult> SaveAsync(IEnumerable<TModel> models)
		{
			var keyProperties = this.GetTypeDescriptorPropertiesWhichAreKeys().ToArray();
			var saveModelResults = new List<SaveModelResult>();

			foreach (var model in models)
			{
				var wasNew = this.IsNew(model);
				this.CheckSaveAuthorization(model, wasNew);

				var result = await this.SaveModelAsync(model);
				var keys = keyProperties.ToDictionary(p => p.Name, p => p.GetValue(model));

				saveModelResults.Add(new SaveModelResult(keys, result, wasNew));
			}

			return new SaveResult(saveModelResults.ToArray());
		}

		protected virtual void CheckSaveAuthorization(TModel model, bool forCreation)
		{
			var authorizedActions = this.GetAuthorizedActions();
			if (!(forCreation
				? authorizedActions.AllowCreate
				: authorizedActions.AllowUpdate))
			{
				throw new AuthorizationException(forCreation ? "Create" : "Update");
			}
		}

		protected virtual void CheckDeleteAuthorization(TModel model)
		{
			if (!this.GetAuthorizedActions().AllowDelete)
			{
				throw new AuthorizationException("Delete");
			}
		}

		/// <summary>
		/// Only the keys properties of models are filled.
		/// </summary>
		public virtual async Task<DeleteResult> DeleteAsync(IEnumerable<TModel> models, PropertyDescriptor[] keyProperties)
		{
			var deleteResults = new List<DeleteModelResult>();

			foreach (var model in models)
			{
				this.CheckDeleteAuthorization(model);

				var result = await this.DeleteModelAsync(model);
				var keys = keyProperties.ToDictionary(p => p.Name, p => p.GetValue(model));

				deleteResults.Add(new DeleteModelResult(keys));
			}
			return new DeleteResult(deleteResults.ToArray());
		}

		Task<SaveResult> IGenericAdminModelConfiguration.SaveAsync(IEnumerable<object> models)
		{
			return this.SaveAsync(models.Cast<TModel>());
		}

		private TModel CreateModel(IDictionary<string, object> keys, PropertyDescriptor[] keyProperties)
		{
			var model = new TModel();
			var invariantCulture = CultureInfo.InvariantCulture;
			var valueConvertService = ServiceProvider.GetRequiredService<IValueConvertService>();

			foreach (var key in keyProperties)
			{
				var convertedValue = valueConvertService.Convert(keys[key.Name], key.PropertyType, invariantCulture);
				key.SetValue(model, convertedValue);
			}

			return model;
		}

		Task<DeleteResult> IGenericAdminModelConfiguration.DeleteAsync(IEnumerable<IDictionary<string, object>> keys)
		{
			var keyProperties = this.GetTypeDescriptorPropertiesWhichAreKeys().ToArray();

			return this.DeleteAsync(
				keys.Select(k => CreateModel(k, keyProperties)),
				keyProperties);
		}

		public virtual AuthorizedActions GetAuthorizedActions()
		{
			return new AuthorizedActions()
			{
				AllowCreate = true,
				AllowUpdate = true,
				AllowDelete = true,
			};
		}

		IAuthorizedActions IGenericAdminModelConfiguration.GetAuthorizedActions()
		{
			return this.GetAuthorizedActions();
		}

		public abstract Task<bool> IsAccessibleAsync();
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class PropertyAttribute : Attribute, IPropertyAttribute, IProperty
	{
		public bool IsKey { get; set; }
		public bool IsEditable { get; set; } = true;

		public void Enrich(Property property)
		{
			Property.Copy(this, property);
		}
	}

	public interface IPropertyAttribute
	{
		void Enrich(Property property);
	}

	public class SimpleDeleteModelResult
	{
	}
}
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public class LocalizableStringsOwnerTypeDescriptionProvider<TOwner> : TypeDescriptionProvider
	{
		public LocalizableStringsOwnerTypeDescriptionProvider()
			: base(TypeDescriptor.GetProvider(typeof(TOwner)))
		{

		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return new LocalizableStringsOwnerTypeDescriptor(
				base.GetTypeDescriptor(objectType, instance));
		}
	}

	public class LocalizableStringsOwnerTypeDescriptor : CustomTypeDescriptor
	{
		public LocalizableStringsOwnerTypeDescriptor(ICustomTypeDescriptor parent)
			: base(parent)
		{
		}

		protected virtual IEnumerable<LocalizableStringPropertyDescriptor> CreateStringProperties(
			PropertyDescriptor[] baseLocalizableStringProperties)
		{
			var culturesService = ApplicationServices.ServiceProvider.GetRequiredService<ICulturesService>();

			return baseLocalizableStringProperties.SelectMany(property =>
				culturesService.AvailableCultures.Select(culture =>
					new LocalizableStringPropertyDescriptor(property, culture.TwoLetterISOLanguageName)));
		}

		public override PropertyDescriptorCollection GetProperties()
		{
			var baseProperties = base.GetProperties().Cast<PropertyDescriptor>().ToList();

			var baseLocalizableStringProperties = baseProperties
				.Where(p => p.Attributes.OfType<LocalizableStringAttribute>().Any())
				.ToArray();

			foreach (var propertyToHide in baseLocalizableStringProperties)
			{
				baseProperties.Remove(propertyToHide);
			}

			return new PropertyDescriptorCollection(baseProperties.Concat(
				this.CreateStringProperties(baseLocalizableStringProperties)).ToArray());
		}
	}

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public class LocalizableStringAttribute : Attribute
	{
	}
}

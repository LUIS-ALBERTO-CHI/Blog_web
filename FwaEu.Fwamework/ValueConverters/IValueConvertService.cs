using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ValueConverters
{
	public interface IValueConvertService
	{
		object Convert(object value, Type destinationType, IFormatProvider provider);
	}

	public class DefaultValueConvertService : IValueConvertService
	{
		public DefaultValueConvertService(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private readonly IServiceProvider _serviceProvider;

		public object Convert(object value, Type destinationType, IFormatProvider provider)
		{
			if (value == null && destinationType.IsGenericType
				&& destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return null;
			}

			destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

			if (value == null && destinationType.IsValueType)
			{
				return null;
			}

			var converter = (IValueConverter)this._serviceProvider.GetService(
				typeof(IValueConverter<>).MakeGenericType(destinationType));

			if (converter != null && converter.CanHandle(value, provider))
			{
				return converter.Convert(value, provider);
			}

			if (destinationType.IsEnum)
			{
				if (value is string)
				{
					return Enum.Parse(destinationType, (string)value);
				}

				var enumUnderlyingType = Enum.GetUnderlyingType(destinationType);
				var valueAsUnderlyingEnumType = enumUnderlyingType.IsAssignableFrom(value.GetType()) ? value
					: System.Convert.ChangeType(value, enumUnderlyingType, provider);

				return Enum.ToObject(destinationType, valueAsUnderlyingEnumType);
			}

			if ( value != null && destinationType.IsAssignableFrom(value.GetType()))
			{
				return value;
			}

			var genericConverter = TypeDescriptor.GetConverter(destinationType);
			if (value != null && genericConverter.CanConvertFrom(value.GetType()))
				return genericConverter.ConvertFrom(value);
			
			return System.Convert.ChangeType(value, destinationType, provider);
		}
	}
}
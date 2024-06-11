using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ValueConverters
{
	public static class ValueConverterExtensions
	{
		public static IServiceCollection AddFwameworkValueConverters(this IServiceCollection services)
		{
			services.AddSingleton<IValueConvertService, DefaultValueConvertService>();
			return services;
		}

		public static TValue ConvertTo<TValue>(this IValueConvertService convertService,
			object value, IFormatProvider provider)
		{
			var convertedValue = convertService.Convert(value, typeof(TValue), provider);
			return convertedValue == null ? default(TValue) : (TValue)convertedValue;
		}
	}
}

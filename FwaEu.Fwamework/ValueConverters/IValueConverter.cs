using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ValueConverters
{
	public interface IValueConverter
	{
		bool CanHandle(object value, IFormatProvider provider);
		object Convert(object value, IFormatProvider provider);
	}

	public interface IValueConverter<TValue> : IValueConverter
	{
		new TValue Convert(object value, IFormatProvider provider);
	}

	public abstract class ValueConverterBase<TValue> : IValueConverter<TValue>
	{
		public abstract bool CanHandle(object value, IFormatProvider provider);
		public abstract TValue Convert(object value, IFormatProvider provider);

		object IValueConverter.Convert(object value, IFormatProvider provider)
		{
			return this.Convert(value, provider);
		}
	}
}

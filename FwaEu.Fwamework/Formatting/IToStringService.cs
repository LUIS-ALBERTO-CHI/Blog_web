using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Formatting
{
	public interface IToStringService
	{
		string ToString(object value, string format, IFormatProvider formatProvider);
	}

	public class DefaultToStringService : IToStringService
	{
		public string ToString(object value, string format, IFormatProvider formatProvider)
		{
			if (value is IFormattable formattable)
			{
				return formattable.ToString(format, formatProvider);
			}

			return value.ToString();
		}
	}
}

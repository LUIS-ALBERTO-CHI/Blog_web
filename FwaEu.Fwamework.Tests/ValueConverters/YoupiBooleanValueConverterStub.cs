using FwaEu.Fwamework.ValueConverters;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.ValueConverters
{
	public class YoupiBooleanValueConverterStub : ValueConverterBase<bool>
	{
		public const string YoupiString = "Youpi";

		public override bool CanHandle(object value, IFormatProvider provider)
		{
			return value is string;
		}

		public override bool Convert(object value, IFormatProvider provider)
		{
			return (string)value == YoupiString;
		}
	}
}

using FwaEu.Fwamework.ValueConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FwaEu.Fwamework.Tests.ValueConverters
{
	[TestClass]
	public class DefaultValueConvertServiceTests
	{
		[TestMethod]
		public void ConvertDateFromString()
		{
			var culture = CultureInfo.InvariantCulture;
			var initialDate = new DateTime(2019, 10, 20, 15, 45, 5, 666);
			var initialDateAsString = initialDate.ToString("O", culture); // NOTE: ISO format with T, like '2009-06-15T13:45:30.0000000'

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var dateConvertedFromString = convertService.ConvertTo<DateTime>(initialDateAsString, culture);

			Assert.AreEqual(initialDate, dateConvertedFromString);
		}

		[TestMethod]
		public void ConvertNullableIntFromNull()
		{
			var culture = CultureInfo.InvariantCulture;

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var value = convertService.ConvertTo<int?>(null, culture);

			Assert.IsNull(value);
		}

		[TestMethod]
		public void ConvertIntFromNull()
		{
			var culture = CultureInfo.InvariantCulture;

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var value = convertService.ConvertTo<int>(null, culture);

			Assert.AreEqual(0, value);
		}

		[TestMethod]
		public void ConvertTimeSpanFromString()
		{
			var culture = CultureInfo.InvariantCulture;
			var initialTimeSpan = new TimeSpan(2, 14, 18);
			var initialTimeSpanAsString = initialTimeSpan.ToString();

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var timeSpanConvertedFromString = convertService.ConvertTo<TimeSpan>(initialTimeSpan, culture);

			Assert.AreEqual(initialTimeSpan, timeSpanConvertedFromString);
		}

		[TestMethod]
		public void ConvertInt32FromInt64()
		{
			var culture = CultureInfo.InvariantCulture;
			int initialInt32 = 325;
			long initialInt32AsInt64 = initialInt32;

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var int64ConvertedFromInt32 = convertService.ConvertTo<Int32>(initialInt32AsInt64, culture);

			Assert.AreEqual(initialInt32, int64ConvertedFromInt32);
		}

		[TestMethod]
		public void ConvertRankFromString()
		{
			var culture = CultureInfo.InvariantCulture;

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var value = convertService.ConvertTo<Rank>(nameof(Rank.Noob), culture);

			Assert.AreEqual(Rank.Noob, value);
		}


		[TestMethod]
		public void ConvertRankFromInt()
		{
			var culture = CultureInfo.InvariantCulture;

			var services = new ServiceCollection();
			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);
			var value = convertService.ConvertTo<Rank>((int)Rank.Noob, culture);

			Assert.AreEqual(Rank.Noob, value);
		}

		[TestMethod]
		public void ConvertBoolFromCustomStringParse()
		{
			var culture = CultureInfo.InvariantCulture;

			var services = new ServiceCollection();
			services.AddTransient<IValueConverter<bool>, YoupiBooleanValueConverterStub>();

			var serviceProvider = services.BuildServiceProvider();

			var convertService = new DefaultValueConvertService(serviceProvider);

			var shouldTrue = convertService.ConvertTo<bool>(YoupiBooleanValueConverterStub.YoupiString, culture);
			Assert.IsTrue(shouldTrue);

			var shouldFalse = convertService.ConvertTo<bool>("no no no", culture);
			Assert.IsFalse(shouldFalse);
		}
	}
}

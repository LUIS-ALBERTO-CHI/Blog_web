using FwaEu.Fwamework.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FwaEu.Fwamework.Tests.Globalization
{
	public class FrenchEnglishCulturesServiceStub : ICulturesService
	{
		public CultureInfo FrenchCulture => this.AvailableCultures[0];
		public CultureInfo EnglishCulture => this.AvailableCultures[1];

		public CultureInfo DefaultCulture => FrenchCulture;

		public CultureInfo[] AvailableCultures { get; } = new[]
			{
				new CultureInfo("fr-FR"),
				new CultureInfo("en-US"),
			};
	}
}

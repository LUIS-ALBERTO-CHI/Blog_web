using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public interface ICulturesService
	{
		CultureInfo DefaultCulture { get; }
		CultureInfo[] AvailableCultures { get; }
	}

	public class DefaultCulturesService : ICulturesService
	{
		public DefaultCulturesService(CultureInfo defaultCulture, CultureInfo[] availableCultures)
		{
			this.DefaultCulture = defaultCulture
				?? throw new ArgumentNullException(nameof(defaultCulture));

			this.AvailableCultures = availableCultures
				?? throw new ArgumentNullException(nameof(availableCultures));

			if (!availableCultures.Contains(defaultCulture))
			{
				throw new ArgumentException(
					"The default culture must be included in available cultures.",
					nameof(availableCultures));
			}
		}

		public CultureInfo DefaultCulture { get; }
		public CultureInfo[] AvailableCultures { get; }
	}
}

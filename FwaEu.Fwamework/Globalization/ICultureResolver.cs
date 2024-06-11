using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public interface ICultureResolver
	{
		CultureInfo ResolveBestCulture(string[] twoLetterIsoCodeUserPreferredCultures);
	}

	public class DefaultCultureResolver : ICultureResolver
	{
		public DefaultCultureResolver(ICulturesService culturesService)
		{
			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));
		}

		private readonly ICulturesService _culturesService;

		public CultureInfo ResolveBestCulture(string[] twoLetterIsoCodeUserPreferredCultures)
		{
			if (twoLetterIsoCodeUserPreferredCultures != null && twoLetterIsoCodeUserPreferredCultures.Any())
			{
				var availableCultures = this._culturesService.AvailableCultures;

				foreach (var twoLetterIsoCodeCulture in twoLetterIsoCodeUserPreferredCultures)
				{
					if (twoLetterIsoCodeCulture.Length != 2)
					{
						throw new ArgumentException(
							"Only two letter iso code cultures are allowed.",
							nameof(twoLetterIsoCodeCulture));
					}

					var matching = availableCultures.FirstOrDefault(
						c => c.TwoLetterISOLanguageName == twoLetterIsoCodeCulture);

					if (matching != null)
					{
						return matching;
					}
				}
			}

			return this._culturesService.DefaultCulture;
		}
	}
}

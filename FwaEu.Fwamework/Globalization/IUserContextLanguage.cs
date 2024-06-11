using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public interface IUserContextLanguage
	{
		CultureInfo GetCulture();
	}

	public class HttpContextUserContextLanguage : IUserContextLanguage
	{
		public HttpContextUserContextLanguage(IHttpContextAccessor httpContextAccessor, ICultureResolver cultureResolver)
		{
			_ = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
			_ = cultureResolver ?? throw new ArgumentNullException(nameof(cultureResolver));
			
			this._resolvedCulture = new Lazy<CultureInfo>(() =>
			{
				var headers = httpContextAccessor.HttpContext.Request.GetTypedHeaders();
				var twoLettersIsoCodePreferredLanguages = (headers.AcceptLanguage == null ? Enumerable.Empty<string>()
					: headers.AcceptLanguage.OrderByDescending(al => al.Quality ?? 1.0) // NOTE: Values come without q= for user expected language, and q= < 1.0 for fallback languages. Example from Chrome with French: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7
						.Select(al => al.Value.Substring(0, 2))
						.Distinct())
					.ToArray();

				return cultureResolver.ResolveBestCulture(twoLettersIsoCodePreferredLanguages);
			});
		}

		private readonly Lazy<CultureInfo> _resolvedCulture;

		public CultureInfo GetCulture()
		{
			return this._resolvedCulture.Value;
		}
	}
}

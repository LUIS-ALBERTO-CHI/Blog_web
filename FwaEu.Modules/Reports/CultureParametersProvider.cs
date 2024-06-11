using FwaEu.Fwamework.Globalization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public class CultureParametersProvider : IParametersProvider
	{
		public const string UserCultureTwoLetterISOLanguageNameKey = "UserCultureTwoLetterISOLanguageName";
		public const string DefaultCultureTwoLetterISOLanguageNameKey = "DefaultCultureTwoLetterISOLanguageName";

		private readonly ICulturesService _culturesService;
		private readonly IUserContextLanguage _userContextLanguage;

		public CultureParametersProvider(
			ICulturesService culturesService,
			IUserContextLanguage userContextLanguage)
		{
			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));

			this._userContextLanguage = userContextLanguage
				?? throw new ArgumentNullException(nameof(userContextLanguage));
		}

		public Task<Dictionary<string, object>> LoadAsync()
		{
			var result = new Dictionary<string, object>()
			{
				{ UserCultureTwoLetterISOLanguageNameKey, this._userContextLanguage.GetCulture().TwoLetterISOLanguageName },
				{ DefaultCultureTwoLetterISOLanguageNameKey, this._culturesService.DefaultCulture.TwoLetterISOLanguageName },
			};

			return Task.FromResult(result);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public interface IUserCultureSettingsProvider
	{
		Task<ICultureSettings> GetCultureSettingsAsync(int userId);
	}

	public interface ICultureSettings
	{
		string LanguageTwoLetterIsoCode { get; }
	}

	public class CultureSettingsModel : ICultureSettings
	{
		public CultureSettingsModel(string languageTwoLetterIsoCode)
		{
			this.LanguageTwoLetterIsoCode = languageTwoLetterIsoCode
				?? throw new ArgumentNullException(nameof(languageTwoLetterIsoCode));
		}

		public string LanguageTwoLetterIsoCode { get; }
	}
}

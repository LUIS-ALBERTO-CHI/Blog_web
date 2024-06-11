using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Users.CultureSettings.Parts
{
	public class CultureSettingsPartModel
	{
		[Required]
		public string LanguageTwoLetterIsoCode { get; set; }
	}
}

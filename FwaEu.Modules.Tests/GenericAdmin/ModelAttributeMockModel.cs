using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	public class ModelAttributeMockModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		//TODO: Implement tests for master data attribute https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/6953
		public int CityId { get; set; }

		[Required]
		//TODO: Implement tests for master data attribute https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/6953
		public DonkeyGender Gender { get; set; }
	}
}

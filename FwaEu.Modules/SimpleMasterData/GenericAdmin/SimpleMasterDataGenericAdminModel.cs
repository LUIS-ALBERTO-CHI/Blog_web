using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FwaEu.Modules.SimpleMasterData.GenericAdmin
{
	public class SimpleMasterDataGenericAdminModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		public string InvariantId { get; set; }

		[Required]
		[LocalizableStringCustomType]
		public Dictionary<string, string> Name { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? UpdatedById { get; set; }

		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? CreatedById { get; set; }

		[Property(IsEditable = false)]
		public DateTime? CreatedOn { get; set; }
	}
}

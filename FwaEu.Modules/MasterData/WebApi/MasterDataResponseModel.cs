using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData.WebApi
{
	public class MasterDataGetChangeInfosResponseModel
	{
		public string MasterDataKey { get; set; }
		public int Count { get; set; }
		public DateTime? MaximumUpdatedOn { get; set; }
	}

	public class MasterDataGetModelsResponseModel
	{
		public string MasterDataKey { get; set; }
		public object[] Values { get; set; }
	}
}
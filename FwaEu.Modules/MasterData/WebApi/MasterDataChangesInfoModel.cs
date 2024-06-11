using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData.WebApi
{
	public class MasterDataChangesInfoModel
	{
		public MasterDataChangesInfoModel(DateTime maximumUpdatedOn, int count)
		{
			this.MaximumUpdatedOn = maximumUpdatedOn;
			this.Count = count;
		}

		public DateTime MaximumUpdatedOn { get; }
		public int Count { get; }
	}
}

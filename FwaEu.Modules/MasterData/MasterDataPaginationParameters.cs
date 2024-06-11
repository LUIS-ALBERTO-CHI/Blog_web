using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public class MasterDataPaginationParameters
	{
		public MasterDataPaginationParameters(int skip, int take)
		{
			this.Skip = skip;
			this.Take = take;
		}

		public int Skip { get; }
		public int Take { get; }
	}
}

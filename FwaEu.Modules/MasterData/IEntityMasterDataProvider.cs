using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public interface IEntityMasterDataProvider: IMasterDataProvider
	{
		Type[] RelatedEntityTypes { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.MasterData
{
	public interface IMasterDataRelatedEntity
	{
		public string MasterDataKey { get; }
		public Type RelatedEntityType { get; }
	}

	public class MasterDataRelatedEntity : IMasterDataRelatedEntity
	{
		public MasterDataRelatedEntity(string key, Type relatedEntityType)
		{
			MasterDataKey = key;
			RelatedEntityType = relatedEntityType;
		}
		public string MasterDataKey { get; }
		public Type RelatedEntityType { get; }
	}
}

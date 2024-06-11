using System;

namespace FwaEu.Modules.SimpleMasterData.MasterData
{
	public class SimpleMasterDataModel
	{
		public SimpleMasterDataModel(int id, string invariantId, string name)
		{
			this.Id = id;
			this.InvariantId = invariantId ?? throw new ArgumentNullException(nameof(invariantId));
			this.Name = name ?? throw new ArgumentNullException(nameof(name));
		}

		public int Id { get; }
		public string InvariantId { get; }
		public string Name { get; }
	}
}

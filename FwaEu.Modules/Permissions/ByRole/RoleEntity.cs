using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.SimpleMasterData;
using System.ComponentModel;


namespace FwaEu.Modules.Permissions.ByRole
{
	[TypeDescriptionProvider(typeof(LocalizableStringsOwnerTypeDescriptionProvider<RoleEntity>))]
	public class RoleEntity : SimpleMasterDataEntityBase
	{
		
	}

	public class RoleEntityRepository : SimpleMasterDataEntityBaseRepository<RoleEntity> 
	{		
	}

	public class RoleEntityClassMap : SimpleMasterDataEntityBaseClassMap<RoleEntity>
	{
		public RoleEntityClassMap() : base()
		{
		}
	}
}

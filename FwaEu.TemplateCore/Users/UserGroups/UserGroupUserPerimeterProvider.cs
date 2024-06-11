using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.Users.UserGroups
{
	public class UserGroupUserPerimeterProvider
		: EntityUserPerimeterProviderBase
			<
			UserGroupPerimeterEntity, UserGroupPerimeterEntityRepository,
			UserGroupEntity, int, UserGroupEntityRepository
			>
	{
		public UserGroupUserPerimeterProvider(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		protected override Expression<Func<UserGroupPerimeterEntity, int>> SelectReferenceEntityId()
		{
			return entity => entity.UserGroup.Id;
		}

		protected override Expression<Func<UserGroupPerimeterEntity, bool>> WhereContainsReferenceEntities(int[] ids)
		{
			return entity => ids.Contains(entity.UserGroup.Id);
		}

		protected override Expression<Func<UserGroupPerimeterEntity, bool>> WhereFullAccess()
		{
			return entity => entity.UserGroup == null;
		}

		protected override Expression<Func<UserGroupPerimeterEntity, bool>> WhereNotFullAccess()
		{
			return entity => entity.UserGroup != null;
		}

		protected override UserGroupPerimeterEntity CreatePerimeterEntity(UserGroupEntity referenceEntity)
		{
			return new UserGroupPerimeterEntity()
			{
				UserGroup = referenceEntity,
			};
		}
	}
}

using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.BlogWeb.FarmManager.Entities;
using FwaEu.BlogWeb.FarmManager.FarmerUserPart;
using FwaEu.BlogWeb.ViewContext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.Users
{
	public class ApplicationUserEntity : UserEntity
		, IPerson
		, ICreationAndUpdateTracked
		, IFarmerPartEntityPropertiesAccessor
		, IApplicationPartEntityPropertiesAccessor
	{
		//NOTE: Sample of project-specific property
		//public string DogName { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		private string _email;
		public string Email
		{
			get { return _email; }
			set
			{
				_email = value;
				Identity = value;
			}
		}

		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

		public string FarmerPseudonym { get; set; }

		public override string ToString()
		{
			return this.ToFullNameString();
		}
	}

	public class ApplicationUserEntityRepository : DefaultUserEntityRepository<ApplicationUserEntity>, IQueryByIds<ApplicationUserEntity, int>
	{
		public IQueryable<ApplicationUserEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}

		public override IQueryable<ApplicationUserEntity> QueryForUsersAdmin()
		{
			var contextService = this.ServiceProvider.GetRequiredService<IViewContextService>();
			var currentUserService = this.ServiceProvider.GetRequiredService<ICurrentUserService>();

			if (currentUserService.User == null || currentUserService.User.Entity.IsAdmin)
			{
				return this.Query();
			}
			var townRegionPerimeterQuery = this.ServiceProvider
				.GetRequiredService<IRepositoryFactory>()
				.Create<TownRegionPerimeterEntityRepository>(this.Session).Query();
			var currentUserPerimeterQuery = townRegionPerimeterQuery.Where(trp => trp.User == currentUserService.User.Entity);

			return this.Query().Where(user => user == currentUserService.User.Entity
				|| !user.IsAdmin
				&& townRegionPerimeterQuery.Any(trp => trp.User == user && currentUserPerimeterQuery.Any(cup => cup.Region == null || trp.Region == null || cup.Region == trp.Region)));
		}
	}

	/// <summary>
	/// Base class map that you can override to change constraints on base properties (Identity...)
	/// </summary>
	public abstract class ApplicationUserEntityClassMap<TUserEntity> : UserEntityClassMap<TUserEntity>
		where TUserEntity : UserEntity
	{

	}

	/// <summary>
	/// Register the class map for UserEntity, using the common overrides from ApplicationUserEntityClassMap<>,
	/// used for UserEntity and also ApplicationUserEntity.
	/// </summary>
	public class UserEntityClassMap : ApplicationUserEntityClassMap<UserEntity>
	{
	}

	/// <summary>
	/// Register the class map for ApplicationUserEntity, which include application-specific properties.
	/// </summary>
	public class ApplicationUserEntityClassMap : ApplicationUserEntityClassMap<ApplicationUserEntity>
	{
		/// <summary>
		/// This name will be shared with UserEntity.Identity, this identity property is used for login/password authentication
		/// </summary>
		public const string IdentityColumnName = "email";

		public ApplicationUserEntityClassMap()
		{
			Map(entity => entity.Email)
				.Column(IdentityColumnName)
				.Not.Nullable()
				.Unique();

			Map(entity => entity.FirstName)
				.Not.Nullable();

			Map(entity => entity.LastName)
			   .Not.Nullable();

			Map(entity => entity.FarmerPseudonym);
			this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}
}

using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	/// <summary>
	/// Values lower than "Active" represents disabled states.
	/// Values equals or greater than "Active" represents enabled states.
	/// </summary>
	public enum UserState
	{
		Disabled = 0,

		/// <summary>
		/// If we add states in the future which represents
		/// either disabled or active states, we could always
		/// do user.State >= UserState.Active.
		/// </summary>
		Active = 1024,
	}

	public class UserEntity : IEntity
	{
		public const string CacheRegionName = nameof(Users);

		public int Id { get; protected set; }
		public string Identity { get; protected set; }
		public bool IsAdmin { get; set; }
		public UserState State { get; set; }

		public bool IsNew()
		{
			return this.Id == 0;
		}

		public override string ToString()
		{
			return this.Identity;
		}
	}

	public class UserEntityClassMapOptions
	{
		public string IdentityColumnName { get; set; }
	}

	public abstract class UserEntityClassMap<TUserEntity> : ClassMap<TUserEntity>
		where TUserEntity : UserEntity
	{
		public UserEntityClassMap()
		{
			var options = ApplicationServices.ServiceProvider
				.GetRequiredService<IOptions<UserEntityClassMapOptions>>()
				.Value;

			ConfigureCache();
			ConfigureTable();
			ConfigureLazyLoad();
			MapId();
			MapIdentity(options);
			MapIsAdmin();
			MapState();
		}

		protected virtual PropertyPart MapState()
		{
			return Map(entity => entity.State)
				.Not.Nullable()
				.Default(((int)UserState.Disabled).ToString())
				.CustomType<UserState>();
		}

		protected virtual PropertyPart MapIsAdmin()
		{
			return Map(entity => entity.IsAdmin);
		}

		protected virtual PropertyPart MapIdentity(UserEntityClassMapOptions options)
		{
			return Map(entity => entity.Identity)
				.Column(options.IdentityColumnName)
				.ReadOnly();
		}

		protected virtual IdentityPart MapId()
		{
			return Id(entity => entity.Id)
				.GeneratedBy.Identity()
				.Column("user_id");
		}

		protected virtual void ConfigureLazyLoad()
		{
			Not.LazyLoad();
		}

		protected virtual void ConfigureTable()
		{
			Table("users"); //NOTE: We can't use FwaConventions here because it require access to application configuration
		}

		protected virtual CachePart ConfigureCache()
		{
			return Cache.Region(UserEntity.CacheRegionName)
				.NonStrictReadWrite();
		}
	}
}

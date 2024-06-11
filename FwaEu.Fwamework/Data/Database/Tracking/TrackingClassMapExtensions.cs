using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Tracking
{
	public static class TrackingClassMapExtensions
	{
		public static void AddCreationTrackedPropertiesIntoMapping<T>(this ClasslikeMapBase<T> mapper)
			where T : ICreationTracked
		{
			mapper.References(entity => entity.CreatedBy).Nullable();
			mapper.Map(entity => entity.CreatedOn).Not.Nullable();
		}

		public static void AddUpdateTrackedPropertiesIntoMapping<T>(this ClasslikeMapBase<T> mapper)
			where T : IUpdateTracked
		{
			mapper.References(entity => entity.UpdatedBy).Nullable();
			mapper.Map(entity => entity.UpdatedOn).Not.Nullable();
		}

		public static void AddCreationAndUpdateTrackedPropertiesIntoMapping<T>(this ClasslikeMapBase<T> mapper)
			where T : ICreationAndUpdateTracked
		{
			mapper.AddCreationTrackedPropertiesIntoMapping();
			mapper.AddUpdateTrackedPropertiesIntoMapping();
		}
	}
}

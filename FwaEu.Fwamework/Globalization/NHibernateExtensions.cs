using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public static class NHibernateExtensions
	{
		public static string[] GetMappedCultureCodes<TEntity>(this ISessionAdapter sessionAdapter, 
			Expression<Func<TEntity, IDictionary>> memberExpression) 
			where TEntity : class, IEntity
		{
			var propertyName = ((MemberExpression)memberExpression.Body).Member.Name;
			return sessionAdapter.GetMappedCultureCodes<TEntity>(propertyName);
		}

		public static string[] GetMappedCultureCodes<TEntity>(this ISessionAdapter sessionAdapter,
			string propertyName)
			where TEntity : class, IEntity
		{
			var nhibernateSession = (NHibernate.Impl.SessionImpl)sessionAdapter.InnerSession;
			var nhibernateMapping = (NHibernate.Persister.Entity.AbstractEntityPersister)nhibernateSession.SessionFactory.GetClassMetadata(typeof(TEntity));
			var mappedProperties = nhibernateMapping.EntityMetamodel.Properties;
			var componentMapping = (NHibernate.Type.ComponentType)mappedProperties.First(x => x.Name == propertyName).Type;
			var mappedCultureCodes = componentMapping.PropertyNames;

			return mappedCultureCodes;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public class EntityDescriptor
	{
		public EntityDescriptor(Type entityType, Type identifierType)
		{
			this.EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
			this.IdentifierType = identifierType ?? throw new ArgumentNullException(nameof(identifierType));
		}

		public Type EntityType { get; }
		public Type IdentifierType { get; }
	}
}

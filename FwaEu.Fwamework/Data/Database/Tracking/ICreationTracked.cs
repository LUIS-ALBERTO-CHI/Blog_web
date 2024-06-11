using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Tracking
{
	public interface ICreatedByTracked : IEntity
	{
		UserEntity CreatedBy { get; set; }
	}

	public interface ICreatedOnTracked : IEntity
	{
		DateTime CreatedOn { get; set; }
	}

	public interface ICreationTracked : ICreatedByTracked, ICreatedOnTracked, IEntity
	{
	}
}

using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database.Tracking
{
	public interface IUpdatedByTracked : IEntity
	{
		UserEntity UpdatedBy { get; set; }
	}

	public interface IUpdatedOnTracked : IEntity
	{
		DateTime UpdatedOn { get; set; }
	}

	public interface IUpdateTracked : IUpdatedByTracked, IUpdatedOnTracked, IEntity
	{
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Users.HistoryPart.Services
{
	public interface IUpdateHistoryPartLoadingModelPropertiesAccessor
	{
		int? UpdatedById { get; }
		DateTime UpdatedOn { get; }
	}

	public interface IHistoryPartLoadingModelPropertiesAccessor : IUpdateHistoryPartLoadingModelPropertiesAccessor
	{
		int? CreatedById { get; }
		DateTime CreatedOn { get; }
	}
}

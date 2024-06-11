using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class DeleteResultsModel
	{
		public DeleteResultModel[] Results { get; set; }

		public static DeleteResultsModel FromDeleteResults(DeleteResult deleteResult)
		{
			return new DeleteResultsModel()
			{
				Results = deleteResult.Results.Select(r => new DeleteResultModel()
				{
					Keys = Helper.CreateDictionary(r.Keys),
				})
				.ToArray(),
			};
		}
	}

	public class DeleteResultModel : ActionOnModelResultModel
	{
	}
}
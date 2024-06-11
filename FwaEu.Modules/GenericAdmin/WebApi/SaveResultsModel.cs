using System.Linq;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class SaveResultsModel
	{
		public SaveResultModel[] Results { get; set; }

		public static SaveResultsModel FromSaveResults(SaveResult saveResult)
		{
			return new SaveResultsModel
			{
				Results = saveResult.Results.Select(r => new SaveResultModel
				{
					SavedData = r.SavedData,
					WasNew = r.WasNew,
					Keys = Helper.CreateDictionary(r.Keys),
				})
				.ToArray(),
			};
		}
	}

	public class SaveResultModel : ActionOnModelResultModel
	{
		public object SavedData { get; set; }
		public bool WasNew { get; set; }
	}
}
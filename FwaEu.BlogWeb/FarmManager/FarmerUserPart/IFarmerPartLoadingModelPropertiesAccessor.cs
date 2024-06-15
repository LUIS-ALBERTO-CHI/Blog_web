using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.FarmerUserPart
{
	public interface IFarmerPartLoadingListModelPropertiesAccessor
	{
		string FarmerPseudonym { get; }
	}

	public interface IFarmerPartLoadingModelPropertiesAccessor
		: IFarmerPartLoadingListModelPropertiesAccessor
	{
	}
}

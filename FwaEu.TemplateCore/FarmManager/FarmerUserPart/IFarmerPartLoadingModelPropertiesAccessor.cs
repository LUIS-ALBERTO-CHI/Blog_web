using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.FarmerUserPart
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

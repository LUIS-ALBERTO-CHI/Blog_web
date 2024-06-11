using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public class ModelUpdatedProcessResultEntry : ProcessResultEntry
	{
		public const string TypeValue = "ModelUpdated";

		public ModelUpdatedProcessResultEntry(string modelAsString)
			   : base(TypeValue, modelAsString)
		{
		}
	}
}

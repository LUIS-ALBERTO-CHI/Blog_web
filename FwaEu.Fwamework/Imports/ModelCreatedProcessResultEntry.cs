using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public class ModelCreatedProcessResultEntry : ProcessResultEntry
	{
		public const string TypeValue = "ModelCreated";

		public ModelCreatedProcessResultEntry(string modelAsString)
			: base(TypeValue, modelAsString)
		{
		}
	}
}

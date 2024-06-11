using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Reports.WebApi
{
	public class EnqueueResultModel
	{
		public EnqueueResultModel(Guid queueGuid)
		{
			this.QueueGuid = queueGuid;
		}

		public Guid QueueGuid { get; }
	}
}

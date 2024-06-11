using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FwaEu.Modules.Reports.BackgroundTasks
{
	public class ReportLoadDataParametersModel
	{
		public ReportLoadDataParametersModel(string reportInvariantId, ReadOnlyDictionary<string, object> parameters)
		{
			this.ReportInvariantId = reportInvariantId ?? throw new ArgumentNullException(nameof(reportInvariantId));
			this.Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
		}

		public string ReportInvariantId { get; }
		public ReadOnlyDictionary<string, object> Parameters { get; }

		public string Serialize()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}

		public static ReportLoadDataParametersModel Deserialize(string value)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<ReportLoadDataParametersModel>(value);
		}
	}
}

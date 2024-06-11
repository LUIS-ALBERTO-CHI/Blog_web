using FwaEu.Fwamework.Temporal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public class DateParametersProvider : IParametersProvider
	{
		private readonly ICurrentDateTime _currentDateTime;

		public DateParametersProvider(ICurrentDateTime currentDateTime)
		{
			this._currentDateTime = currentDateTime
				?? throw new ArgumentNullException(nameof(currentDateTime));
		}

		public Task<Dictionary<string, object>> LoadAsync()
		{
			var now = this._currentDateTime.Now;

			var result = new Dictionary<string, object>()
			{
				{ "DateTimeNow", now },
				{ "DateTimeToday", now.Date },
			};

			return Task.FromResult(result);
		}
	}
}

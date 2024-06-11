using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.ProcessResults
{
	public class ErrorProcessResultEntry : ProcessResultEntry
	{
		private static IEnumerable<Exception> InlineException(Exception ex)
		{
			if (ex != null)
			{
				yield return ex;

				if (ex.InnerException != null)
				{
					foreach (var inner in InlineException(ex.InnerException))
					{
						yield return inner;
					}
				}
			}
		}

		public static ErrorProcessResultEntry FromException(string content, Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException(nameof(exception));
			}

			var details = InlineException(content == null ? exception.InnerException : exception)
				.Select(ex => ex.Message)
				.ToArray();

			return new ErrorProcessResultEntry(content ?? exception.Message,
				details.Any() ? details : null);
		}

		public static ErrorProcessResultEntry FromException(Exception exception)
		{
			return FromException(null, exception);
		}

		public ErrorProcessResultEntry(string content, string[] details = null) : base("Error", content, details)
		{
		}
	}
}

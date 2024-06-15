using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Photo.WebApi
{
	public enum NotFoundType
	{
		Farm,
		Photo
	}

	public class NotFoundApiModel
	{
		public NotFoundApiModel(NotFoundType type, string message)
		{
			this.Type = type;
			this.Message = message ?? throw new ArgumentNullException(nameof(message));
		}

		public NotFoundType Type { get; }
		public string Message { get; }
	}
}

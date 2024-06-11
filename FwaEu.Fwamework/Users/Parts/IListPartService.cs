using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.Parts
{
	public interface IListPartService
	{
		Task<Dictionary<string, object>> LoadAllPartsAsync(UserListModel model);
	}

	public class DefaultListPartService : IListPartService
	{
		public DefaultListPartService(IEnumerable<IListPartHandler> partHandlers)
		{
			this._partHandlers = partHandlers
				?? throw new ArgumentNullException(nameof(partHandlers));
		}

		private readonly IEnumerable<IListPartHandler> _partHandlers;

		public async Task<Dictionary<string, object>> LoadAllPartsAsync(UserListModel model)
		{
			var parts = new Dictionary<string, object>();

			foreach (var partHandler in this._partHandlers)
			{
				var part = await partHandler.LoadAsync(model);
				parts.Add(partHandler.Name, part);
			}

			return parts;
		}
	}
}

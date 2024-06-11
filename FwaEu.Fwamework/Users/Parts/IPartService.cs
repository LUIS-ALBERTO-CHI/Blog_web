using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users.Parts
{
	public interface IPartService
	{
		Task<Dictionary<string, object>> LoadAllPartsAsync();
		Task<IPartSaveResult> SaveAllPartsAsync(Dictionary<string, object> modelsByPartNames);
		Dictionary<string, IPartHandler> GetSaveHandlerByPartName();
	}


	public interface IPartSaveResult
	{
		Task FinalizeAfterEntitySaveAsync();
		Task FinalizeAfterTransactionAsync();
	}

	public class CompositePartSaveResult : IPartSaveResult
	{
		private IPartSaveResult[] _results;

		public CompositePartSaveResult(IPartSaveResult[] results)
		{
			this._results = results
				?? throw new ArgumentNullException(nameof(results));
		}

		public async Task FinalizeAfterEntitySaveAsync()
		{
			foreach (var result in this._results)
			{
				await result.FinalizeAfterEntitySaveAsync();
			}
		}

		public async Task FinalizeAfterTransactionAsync()
		{
			foreach (var result in this._results)
			{
				await result.FinalizeAfterTransactionAsync();
			}
		}
	}

	public class PartSaveResult : IPartSaveResult
	{
		private Func<Task> _afterSaveTask;
		private Func<Task> _afterTransactionTask;

		public PartSaveResult(Func<Task> afterSaveTask = null, Func<Task> afterTransactionTask = null)
		{
			this._afterSaveTask = afterSaveTask;
			this._afterTransactionTask = afterTransactionTask;
		}

		public async Task FinalizeAfterEntitySaveAsync()
		{
			if (this._afterSaveTask != null)
			{
				await this._afterSaveTask();
			}
		}

		public async Task FinalizeAfterTransactionAsync()
		{
			if (this._afterTransactionTask != null)
			{
				await this._afterTransactionTask();
			}
		}
	}

	public class DefaultPartService : IPartService
	{
		public DefaultPartService(IEnumerable<IPartHandler> partHandlers)
		{
			this._partHandlers = partHandlers ?? throw new ArgumentNullException(nameof(partHandlers));
		}

		private readonly IEnumerable<IPartHandler> _partHandlers;

		public async Task<Dictionary<string, object>> LoadAllPartsAsync()
		{
			var parts = new Dictionary<string, object>();

			foreach (var partHandler in this._partHandlers)
			{
				var part = await partHandler.LoadAsync();
				parts.Add(partHandler.Name, part);
			}

			return parts;
		}

		public async Task<IPartSaveResult> SaveAllPartsAsync(Dictionary<string, object> modelsByPartNames)
		{
			var results = new List<IPartSaveResult>();
			var partHandlersToUse = this._partHandlers
				.Where(ph =>
				{
					var isRequested = modelsByPartNames.ContainsKey(ph.Name);
					if (isRequested && !ph.IsEditable)
					{
						throw new NotSupportedException($"The part {ph.Name} is not editable.");
					}
					return isRequested;
				});

			foreach (var partHandler in partHandlersToUse)
			{
				var userEditable = await partHandler.CurrentUserCanEditAsync();
				if (!userEditable)
				{
					throw new NotSupportedException($"The current user has no permission for editing {partHandler.Name} part.");
				}

				var partResult = await partHandler.SaveAsync(modelsByPartNames[partHandler.Name]);
				if (partResult != null)
				{
					results.Add(partResult);
				}
			}

			return new CompositePartSaveResult(results.ToArray());
		}

		public Dictionary<string, IPartHandler> GetSaveHandlerByPartName()
		{
			return this._partHandlers.Where(ph => ph.IsEditable)
				.ToDictionary(ph => ph.Name);
		}
	}
}

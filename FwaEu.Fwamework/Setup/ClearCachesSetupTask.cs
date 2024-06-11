using FwaEu.Fwamework.Caching;
using FwaEu.Fwamework.ProcessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup
{
	public class ClearCachesSetupTask : ISetupTask
	{
		private readonly IEnumerable<ICacheManager> _cacheManagers;

		public ClearCachesSetupTask(IEnumerable<ICacheManager> cacheManagers)
		{
			_cacheManagers = cacheManagers ?? throw new ArgumentNullException(nameof(cacheManagers));
		}

		public string Name => "ClearCaches";

		public Type ArgumentsType => null;

		public async Task<ISetupTaskResult> ExecuteAsync(object arguments)
		{
			var result = new ProcessResult();
			var context = result.CreateContext($"Application caches clear", "ClearCacheSetupTask");

			await Task.WhenAll(_cacheManagers.Select(cacheManager =>
			{
				return cacheManager.ClearAsync()
				.ContinueWith(c => context.Add(new InfoProcessResultEntry($"{cacheManager.GetType().Name}: Cleared")));
			}).ToArray());

			return new NoDataSetupTaskResult(result);
		}
	}
}

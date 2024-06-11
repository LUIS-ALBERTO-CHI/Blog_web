using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IAsyncReportDataStoreService
	{
		Task<string> SetAsync(ReportDataModel data);
		Task<ReportDataModel> GetAsync(string key);
		Task CleanUpAsync();
	}

	public class MemoryCacheAsyncReportDataStoreService : IAsyncReportDataStoreService
	{
		private readonly IMemoryCache _memoryCache;

		public MemoryCacheAsyncReportDataStoreService(IMemoryCache memoryCache)
		{
			this._memoryCache = memoryCache
				?? throw new ArgumentNullException(nameof(memoryCache));
		}

		public Task CleanUpAsync()
		{
			//NOTE: Nothing to do in current implementation, because the clean up behavior is provided by the underlying IMemoryCache
			return Task.CompletedTask;
		}

		public Task<ReportDataModel> GetAsync(string key)
		{
			return Task.FromResult(this._memoryCache.Get<ReportDataModel>(key));
		}

		public Task<string> SetAsync(ReportDataModel data)
		{
			var key = Guid.NewGuid().ToString();
			this._memoryCache.Set(key, data);
			return Task.FromResult(key);
		}
	}
}

using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.BlogWeb.FarmManager.Entities;
using FwaEu.BlogWeb.ViewContext.WebApi;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.ViewContext
{
	public enum ViewContextLoadResult
	{
		OutOfPerimeter,
		Loaded
	}

	public interface IViewContextService
	{
		ViewContextModel Current { get; }
		Task<ViewContextLoadResult> LoadAsync();
	}

	public class HttpHeaderViewContextService : IViewContextService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly MainSessionContext _sessionContext;

		public HttpHeaderViewContextService(
			IHttpContextAccessor httpContextAccessor,
			MainSessionContext sessionContext)
		{
			this._httpContextAccessor = httpContextAccessor
				?? throw new ArgumentNullException(nameof(httpContextAccessor));

			this._sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		public ViewContextModel Current { get; private set; }

		private static ViewContextApiModel Deserialize(string value)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<ViewContextApiModel>(value);
		}

		public async Task<ViewContextLoadResult> LoadAsync()
		{
			var headerValue = this._httpContextAccessor.HttpContext.Request.Headers["View-Context"].FirstOrDefault();

			if (!String.IsNullOrEmpty(headerValue))
			{
				var apiModel = Deserialize(headerValue);

				try
				{

					if (apiModel != null)
					{

						this.Current = new ViewContextModel(await this._sessionContext.RepositorySession
							.GetOrNotFoundExceptionAsync<TownRegionEntity, int, TownRegionEntityRepository>(apiModel.RegionId));
					}
				}
				catch (NotFoundException)
				{
					return ViewContextLoadResult.OutOfPerimeter;
				}
			}

			return ViewContextLoadResult.Loaded; // NOTE: Null is an acceptable value, when there is no header
		}
	}
}
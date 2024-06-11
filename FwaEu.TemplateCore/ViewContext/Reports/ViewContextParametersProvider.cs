using FwaEu.Modules.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.ViewContext.Reports
{
	public class ViewContextParametersProvider : IParametersProvider
	{
		private readonly IViewContextService _viewContextService;

		public ViewContextParametersProvider(IViewContextService viewContextService)
		{
			this._viewContextService = viewContextService
				?? throw new ArgumentNullException(nameof(viewContextService));
		}

		public Task<Dictionary<string, object>> LoadAsync()
		{
			return Task.FromResult(
				this._viewContextService.Current?.ToDictionary()
				?? new Dictionary<string, object>());
		}
	}
}

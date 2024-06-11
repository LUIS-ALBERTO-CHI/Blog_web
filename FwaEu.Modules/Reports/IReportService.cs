using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public interface IReportService
	{
		Task<ReportLoadResultModel<ServiceReportModel>> FindByInvariantIdAsync(
			string invariantId, CultureInfo culture, bool checkPerimeter = true);

		Task<IEnumerable<ReportLoadResultModel<ReportListItemModel>>> GetAllAsync(CultureInfo culture);
	}

	public class DefaultReportService : IReportService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IEnumerable<IReportProviderFactory> _reportProviderFactories;

		public DefaultReportService(
			IServiceProvider serviceProvider,
			IEnumerable<IReportProviderFactory> reportProviderFactories)
		{
			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this._reportProviderFactories = reportProviderFactories
				?? throw new ArgumentNullException(nameof(reportProviderFactories));
		}

		private IEnumerable<IReportProvider> GetProviders()
		{
			return this._reportProviderFactories
				.Select(pf => pf.Create(this._serviceProvider));
		}

		public async Task<ReportLoadResultModel<ServiceReportModel>> FindByInvariantIdAsync(
			string invariantId, CultureInfo userCulture, bool checkPerimeter = true)
		{
			if (checkPerimeter)
			{
				var reportAccessService = this._serviceProvider.GetRequiredService<IReportAccessService>();
				var currentUserService = this._serviceProvider.GetRequiredService<ICurrentUserService>();

				var accessible = await reportAccessService.IsAccessibleAsync(
					invariantId, currentUserService.User.Entity.Id);

				if (!accessible)
				{
					return null;
				}
			}

			foreach (var provider in this.GetProviders())
			{
				var model = await provider.FindByInvariantIdAsync(invariantId,
					userCulture);

				if (model != null)
				{
					return model;
				}
			}

			return null;
		}

		public async Task<IEnumerable<ReportLoadResultModel<ReportListItemModel>>> GetAllAsync(CultureInfo culture)
		{
			var tasks = this.GetProviders().Select(provider => provider
				.GetAllAsync(culture));

			await Task.WhenAll(tasks);

			var reportAccessService = this._serviceProvider.GetRequiredService<IReportAccessService>();
			var currentUserService = this._serviceProvider.GetRequiredService<ICurrentUserService>();

			var access = await reportAccessService.GetAccessAsync(
				currentUserService.User.Entity.Id);

			var results = tasks.SelectMany(task => task.Result);

			if (!access.HasFullAccess)
			{
				results = results.Join(access.AccessibleReportInvariantIds,
					result => result.Report.InvariantId, aii => aii,
					(result, aii) => result);
			}

			return results.ToArray();
		}
	}
}

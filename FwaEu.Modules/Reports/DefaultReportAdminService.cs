using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Reports
{
	public class DefaultReportAdminService : IReportAdminService
	{
		private readonly IEnumerable<IReportAdminProvider> _reportAdminProviders;
		private readonly IServiceProvider _serviceProvider;

		public DefaultReportAdminService(IEnumerable<IReportAdminProvider> reportAdminProviders,
										 IServiceProvider serviceProvider)
		{
			this._reportAdminProviders = reportAdminProviders
				?? throw new ArgumentNullException(nameof(reportAdminProviders));

			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public async Task<ReportAdminModel> FindByInvariantIdAsync(string invariantId)
		{
			foreach (var provider in _reportAdminProviders)
			{
				if (provider.SupportsSave)
				{
					var report = await provider.GetByInvariantIdAsync(invariantId);
					if (report != null)
					{
						return report.Report;
					}
				}
			}

			return null;
		}

		public async Task<string> GetJsonByInvariantIdAsync(string invariantId)
		{
			foreach (var provider in _reportAdminProviders)
			{
				if (await provider.ReportExistsAsync(invariantId))
				{
					return await provider.GetJsonByInvariantIdAsync(invariantId);
				}
			}

			return null;
		}

		public async Task SaveAsync(string invariantId, ReportAdminModel model)
		{
			var providers = _reportAdminProviders.ToArray();
			IReportAdminProvider reportProvider = null;

			foreach (var provider in providers)
			{
				if (await provider.ReportExistsAsync(invariantId))
				{
					if (!provider.SupportsSave)
					{
						throw new NotSupportedException("Save is not supported for this report.");
					}
					reportProvider = provider;
					break;
				}
			}
			reportProvider ??= providers.First(x => x.SupportsSave);
			await reportProvider.SaveAsync(invariantId, model);
		}

		public async Task<ReportLoadResultModel<ReportAdminListItemModel>[]> GetAllAsync(CultureInfo culture)
		{
			var tasks = _reportAdminProviders.ToArray().Select(provider => provider
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

		public async Task DeleteAsync(string invariantId)
		{
			var providers = _reportAdminProviders
				.Where(x => x.SupportsSave)
				.ToArray();
			IReportAdminProvider reportProvider = null;

			foreach (var provider in providers)
			{
				if (await provider.ReportExistsAsync(invariantId))
				{
					reportProvider = provider;
					break;
				}
			}
			if(reportProvider == null)
				throw new NotSupportedException($"No provider found for report {invariantId}.");
			await reportProvider.DeleteAsync(invariantId);
		}
	}
}

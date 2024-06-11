using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Reports;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsProvidersByEntities
{
	public class EntitiesReportProviderFactory : IReportProviderFactory
	{
		public IReportProvider Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<EntitiesReportProvider>();
		}
	}

	public class EntitiesReportProvider : IReportProvider
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly ICulturesService _culturesService;

		public EntitiesReportProvider(ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory, ICulturesService culturesService)
		{
			this._sessionAdapterFactory = sessionAdapterFactory 
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));

			this._repositoryFactory = repositoryFactory 
				?? throw new ArgumentNullException(nameof(repositoryFactory));

			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));
		}

		public async Task<ReportLoadResultModel<ServiceReportModel>> FindByInvariantIdAsync(
			string invariantId, CultureInfo userCulture) 
		{
			using (var session = _sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportEntityRepository>(session);
				var entity = await repository.FindByInvariantIdAsync(invariantId);

				if (entity == null)
					return null;

				string GetString(ReportLocalizableString localizableString)
				{
					if (userCulture != null)
					{
						return localizableString.GetString(
							userCulture.TwoLetterISOLanguageName,
							this._culturesService.DefaultCulture.TwoLetterISOLanguageName);
					}

					return localizableString.ToStringFirstValue();
				}

				var model = JsonConvert.DeserializeObject<ReportModel<ReportLocalizableString>>(entity.Json);

				var defaultViews = model.DefaultViews.ToDictionary(kv => kv.Key,
					kv => kv.Value.Select(view => new ReportViewModel<string>(view.IsDefault,
						GetString(view.Name), view.Value)).ToArray());

				var report = new ServiceReportModel(entity.InvariantId, GetString(model.Name), GetString(model.Description),
				model.CategoryInvariantId, model.IsAsync, model.Icon, model.Navigation,
				model.DataSource,
				model.Filters,
				model.Properties, defaultViews);
				
				return new ReportLoadResultModel<ServiceReportModel>(report);
			}
		}

		public async Task<IEnumerable<ReportLoadResultModel<ReportListItemModel>>> GetAllAsync(CultureInfo userCulture)
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var reports = await this._repositoryFactory
					.Create<ReportEntityRepository>(session)
					.Query().Select(x => new
					{
						x.Json,
						x.InvariantId,
					}
					).ToListAsync();

				return reports.Select(report =>
				{
					var model = JsonConvert.DeserializeObject<ReportModel<ReportLocalizableString>>(report.Json);

					return new ReportLoadResultModel<ReportListItemModel>(
					new ReportListItemModel(report.InvariantId,
						model.Filters.Any(),
						model.Name[userCulture.TwoLetterISOLanguageName] 
							?? model.Name[this._culturesService.DefaultCulture.TwoLetterISOLanguageName],
						model.Description[userCulture.TwoLetterISOLanguageName] 
							?? model.Description[this._culturesService.DefaultCulture.TwoLetterISOLanguageName],
						model.CategoryInvariantId, model.Navigation, model.IsAsync, model.Icon));
				}).ToArray();
			}
		}
	}
}

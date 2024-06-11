using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Reports;
using FwaEu.Modules.ReportsProvidersByEntities;
using Newtonsoft.Json;
using NHibernate.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsAdminProvidersByEntities
{
	public class ReportAdminProviderByEntities : IReportAdminProvider
	{
		private readonly ISessionAdapterFactory _sessionAdapterFactory;
		private readonly IRepositoryFactory _repositoryFactory;
		private readonly ICulturesService _culturesService;

		public ReportAdminProviderByEntities(ISessionAdapterFactory sessionAdapterFactory,
			IRepositoryFactory repositoryFactory, ICulturesService culturesService)
		{
			this._sessionAdapterFactory = sessionAdapterFactory
				?? throw new ArgumentNullException(nameof(sessionAdapterFactory));
			this._repositoryFactory = repositoryFactory
				?? throw new ArgumentNullException(nameof(repositoryFactory));
			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));
		}

		public bool SupportsSave => true;

		public async Task<bool> ReportExistsAsync(string invariantId)
		{
			using (var session = _sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportEntityRepository>(session);
				return await repository.Query().AnyAsync(x => x.InvariantId == invariantId);
			}
		}

		public async Task<string> GetJsonByInvariantIdAsync(string invariantId)
		{
			using (var session = _sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportEntityRepository>(session);
				var entity = await repository.FindByInvariantIdAsync(invariantId);

				if (entity == null)
					return null;

				return entity.Json;
			}
		}
		public async Task<ReportLoadResultModel<ReportAdminListItemModel>[]> GetAllAsync(CultureInfo userCulture)
		{
			using (var session = _sessionAdapterFactory.CreateStatefulSession())
			{
				var reports = await this._repositoryFactory
					.Create<ReportEntityRepository>(session)
					.Query().Select(x => new
					{
						x.Json,
						x.InvariantId,
						SupportsSave = this.SupportsSave,
					}
					).ToListAsync();

				return reports.Select(report =>
				{
					var model = JsonConvert.DeserializeObject<ReportModel<ReportLocalizableString>>(report.Json);

					return new ReportLoadResultModel<ReportAdminListItemModel>(
					new ReportAdminListItemModel(
						model.Name[userCulture.TwoLetterISOLanguageName]
							?? model.Name[this._culturesService.DefaultCulture.TwoLetterISOLanguageName],
						model.CategoryInvariantId,
						report.InvariantId,
						model.Icon,
						this.SupportsSave));
				}).ToArray();
			}
		}
		public async Task<ReportLoadResultModel<ReportAdminModel>> GetByInvariantIdAsync(string invariantId)
		{
			using (var session = _sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportEntityRepository>(session);
				var entity = await repository.FindByInvariantIdAsync(invariantId);

				if (entity == null)
					return null;

				var report = JsonConvert.DeserializeObject<ReportModel<ReportLocalizableString>>(entity.Json);

				var model = new ReportAdminModel(
					report.Name,
					report.Description,
					report.CategoryInvariantId,
					report.IsAsync,
					report.Icon,
					new ReportAdminNavigationModel(new ReportAdminNavigationItemModel(report.Navigation.Menu.Visible, report.Navigation.Menu.Index),
						new ReportAdminNavigationItemModel(report.Navigation.Summary.Visible, report.Navigation.Summary.Index)),
					new ReportAdminDataSourceModel(report.DataSource.Type, report.DataSource.Argument),
					report.Filters.Select(x => new ReportAdminFilterModel(x.InvariantId, x.IsRequired)).ToArray(),
					report.Properties.Select(x => new ReportAdminPropertyModel(x.Name, x.FieldInvariantId)).ToArray(),
					report.DefaultViews.ToDictionary(x => x.Key,
						x => x.Value.Select(v => new ReportAdminViewModel(v.IsDefault, v.Name, v.Value)).ToArray()));

				return new ReportLoadResultModel<ReportAdminModel>(model);
			}
		}

		public async Task SaveAsync(string invariantId, ReportAdminModel report)
		{
			using (var session = _sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportEntityRepository>(session);
				var entity = await repository.FindByInvariantIdAsync(invariantId) ?? new ReportEntity()
				{
					InvariantId = invariantId,
					IsActive = true,
				};

				var json = JsonConvert.SerializeObject(report, Formatting.Indented);
				entity.Json = json;
				await repository.SaveOrUpdateAsync(entity);
				await session.FlushAsync();
			}
		}

		public async Task DeleteAsync(string invariantId)
		{
			using (var session = this._sessionAdapterFactory.CreateStatefulSession())
			{
				var repository = this._repositoryFactory.Create<ReportEntityRepository>(session);
				var entity = await repository.FindByInvariantIdAsync(invariantId);

				if (entity == null)
					throw new ReportNotFoundException($"Report {invariantId} not found for deletion");

				await repository.DeleteAsync(entity);
				await session.FlushAsync();
			}
		}
	}
}

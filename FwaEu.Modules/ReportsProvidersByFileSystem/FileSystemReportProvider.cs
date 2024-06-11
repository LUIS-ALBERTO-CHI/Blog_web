using FwaEu.Modules.Reports;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework.Configuration;
using System.IO;
using Microsoft.Extensions.Options;
using FwaEu.Fwamework.Globalization;

namespace FwaEu.Modules.ReportsProvidersByFileSystem
{
	public class FileSystemReportProviderFactory : IReportProviderFactory
	{
		public IReportProvider Create(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetService<FileSystemReportProvider>();
		}
	}

	public class FileSystemReportProvider : IReportProvider
	{
		private readonly IPathFileProvider _pathFileProvider;
		private readonly ReportsProvidersByFileSystemOptions _settings;
		private readonly ICulturesService _culturesService;

		public FileSystemReportProvider(
			IPathFileProvider pathFileProvider,
			IOptions<ReportsProvidersByFileSystemOptions> settings,
			ICulturesService culturesService)
		{
			this._pathFileProvider = pathFileProvider
				?? throw new ArgumentNullException(nameof(pathFileProvider));

			this._settings = (settings ?? throw new ArgumentNullException(nameof(settings))).Value;

			this._culturesService = culturesService
				?? throw new ArgumentNullException(nameof(culturesService));
		}

		public static string ExtractInvariantIdFromFileName(string fileName)
		{
			return fileName.Split(".")[0];
		}

		private async static Task<(FileSystemReportModel Model, string InvariantId)> LoadReportAsync(FileInfo fileInfo)
		{
			var json = await File.ReadAllTextAsync(fileInfo.FullName);
			var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FileSystemReportModel>(json);
			var invariantId = ExtractInvariantIdFromFileName(fileInfo.Name);

			return (model, invariantId);
		}

		private IEnumerable<FileInfoByPathEntry> GetFiles()
		{
			return this._pathFileProvider.GetFiles(this._settings.StoragePaths);
		}

		public async Task<ReportLoadResultModel<ServiceReportModel>> FindByInvariantIdAsync(
			string invariantId, CultureInfo userCulture)
		{
			var file = this.GetFiles()
				.FirstOrDefault(file => ExtractInvariantIdFromFileName(file.FileInfo.Name) == invariantId);

			if (file == null)
			{
				return null;
			}

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

			var result = await LoadReportAsync(file.FileInfo);
			var model = result.Model;

			var defaultViews = model.DefaultViews.ToDictionary(kv => kv.Key,
				kv => kv.Value.Select(view => new ReportViewModel<string>(view.IsDefault,
					GetString(view.Name), view.Value)).ToArray());

			var report = new ServiceReportModel(
				invariantId,
				GetString(model.Name), GetString(model.Description),
				model.CategoryInvariantId, model.IsAsync, model.Icon, model.Navigation, 
				model.DataSource,
				model.Filters, model.Properties, defaultViews);

			return new ReportLoadResultModel<ServiceReportModel>(report);
		}

		public async Task<IEnumerable<ReportLoadResultModel<ReportListItemModel>>> GetAllAsync(CultureInfo userCulture)
		{
			var tasks = GetFiles()
				.Select(file => LoadReportAsync(file.FileInfo))
				.ToArray();

			await Task.WhenAll(tasks);

			string GetString(ReportLocalizableString localizableString)
			{
				return localizableString.GetString(
					userCulture.TwoLetterISOLanguageName,
					this._culturesService.DefaultCulture.TwoLetterISOLanguageName);
			}

			return tasks.Select(task =>
			{
				var model = task.Result.Model;

				return new ReportLoadResultModel<ReportListItemModel>(
					new ReportListItemModel(task.Result.InvariantId,
						task.Result.Model.Filters.Any(),
						GetString(model.Name), GetString(model.Description),
						model.CategoryInvariantId, model.Navigation, model.IsAsync, model.Icon));
			}).ToArray();
		}
	}
}

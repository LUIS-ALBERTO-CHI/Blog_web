using FwaEu.Fwamework.Configuration;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Reports;
using FwaEu.Modules.ReportsProvidersByFileSystem;
using Microsoft.Extensions.Options;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.ReportsAdminProvidersByFileSystem
{
	public class ReportAdminProviderByFileSystem : IReportAdminProvider
	{
		private readonly IPathFileProvider _pathFileProvider; 
		private readonly ReportsProvidersByFileSystemOptions _settings;
		private readonly ICulturesService _culturesService;
		public bool SupportsSave => false;

		public ReportAdminProviderByFileSystem(
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

		public Task<bool> ReportExistsAsync(string invariantId)
		{
			var files = this._pathFileProvider.GetFiles(this._settings.StoragePaths);
			var report = files.FirstOrDefault(x => FileSystemReportProvider.ExtractInvariantIdFromFileName(x.FileInfo.Name) == invariantId);

			return Task.FromResult(report != null);
		}
		public Task<ReportLoadResultModel<ReportAdminModel>> GetByInvariantIdAsync(string invariantId)
		{
			throw new NotSupportedException();
		}

		public Task SaveAsync(string invariantId, ReportAdminModel report)
		{
			throw new NotSupportedException();
		}

		public async Task<string> GetJsonByInvariantIdAsync(string invariantId)
		{
			var files = this._pathFileProvider.GetFiles(this._settings.StoragePaths);
			var reportFile = files.FirstOrDefault(x => FileSystemReportProvider.ExtractInvariantIdFromFileName(x.FileInfo.Name) == invariantId);
			if (reportFile == null)
				return null;

			return await File.ReadAllTextAsync(reportFile.FileInfo.FullName);
		}

		public Task DeleteAsync(string invariantId)
		{
			throw new NotSupportedException();
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
		public async Task<ReportLoadResultModel<ReportAdminListItemModel>[]> GetAllAsync(CultureInfo userCulture)
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

				return new ReportLoadResultModel<ReportAdminListItemModel>(
					new ReportAdminListItemModel(
						GetString(model.Name),
						model.CategoryInvariantId,
						task.Result.InvariantId,
						model.Icon,
						this.SupportsSave));
			}).ToArray();
		}
	}
}

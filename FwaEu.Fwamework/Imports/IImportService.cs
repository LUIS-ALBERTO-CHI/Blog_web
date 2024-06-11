using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public interface IImportService
	{
		Task ImportAllFilesAsync(ImportContext context);
	}

	public class DefaultImportService : IImportService
	{
		public DefaultImportService(IEnumerable<IImportSessionFactory> importSessionFactories)
		{
			this._importSessionFactories = importSessionFactories;
		}

		private readonly IEnumerable<IImportSessionFactory> _importSessionFactories;

		private async Task<(IEnumerable<IImportFileSession> Sessions, IImportFile[] RemainingFiles)>
			CreateImportFileSessionsAsync(ImportContext context)
		{
			var filesToImport = context.Files.ToList();
			var sessions = new List<IImportFileSession>();

			foreach (var factory in this._importSessionFactories)
			{
				var importSession = await factory.CreateImportSessionAsync(context, filesToImport.ToArray());
				if (importSession != null)
				{
					sessions.Add(importSession);
					foreach (var importableFile in importSession.OrderedFiles)
					{
						filesToImport.Remove(importableFile);
					}
				}

				if (!filesToImport.Any())
				{
					break;
				}
			}

			return (sessions, filesToImport.ToArray());
		}

		public async Task ImportAllFilesAsync(ImportContext context)
		{
			var (sessions, remainingFiles) = await this.CreateImportFileSessionsAsync(context);

			if (remainingFiles.Any())
			{
				throw new NoImporterFoundException(
					remainingFiles.Select(f => f.Name).ToArray());
			}

			foreach (var session in sessions)
			{
				await session.ImportAsync();
			}
		}
	}

	public class NoImporterFoundException : Exception
	{
		public NoImporterFoundException(string[] fileNames)
			: base($"No importer found for the following files: {String.Join(", ", fileNames)}")
		{
			this.FileNames = fileNames;
		}

		public string[] FileNames { get; }
	}
}

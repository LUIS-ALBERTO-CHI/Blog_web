using FwaEu.Fwamework.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Importers.SqlImporter
{
	public class SqlImportFileSession : IImportFileSession
	{
		public SqlImportFileSession(SqlImporter importer, ImportContext context, IImportFile[] orderedFiles)
		{
			this._importer = importer ?? throw new ArgumentNullException(nameof(importer));
			this._context = context ?? throw new ArgumentNullException(nameof(context));
			this.OrderedFiles = orderedFiles ?? throw new ArgumentNullException(nameof(orderedFiles));
		}

		private readonly SqlImporter _importer;
		private readonly ImportContext _context;
		public IImportFile[] OrderedFiles { get; }

		public async Task ImportAsync()
		{
			await this._importer.ImportAsync(this._context, this.OrderedFiles);
		}
	}
}

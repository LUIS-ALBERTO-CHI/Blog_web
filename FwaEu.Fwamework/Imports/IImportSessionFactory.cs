using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Imports
{
	public interface IImportSessionFactory
	{
		Task<IImportFileSession> CreateImportSessionAsync(ImportContext context, IImportFile[] files);
	}

	public interface IImportFileSession
	{
		IImportFile[] OrderedFiles { get; }
		Task ImportAsync();
	}
}

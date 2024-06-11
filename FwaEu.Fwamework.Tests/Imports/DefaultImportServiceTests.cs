using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Tests.Imports
{
	[TestClass]
	public class DefaultImportServiceTests
	{
		[TestMethod]
		public async Task NoImporterMatchFile_MustFail()
		{
			var emptyImportService = new DefaultImportService(new IImportSessionFactory[] { });

			await using (var context = new ImportContext(new ProcessResult(), new IImportFile[]
				{
					new ImportFileFake("NoImporterForMe.wootwoot"),
					new ImportFileFake("NoImporterForMeToo.balanceyourpork")
				}))
			{

				await Assert.ThrowsExceptionAsync<NoImporterFoundException>(
					() => emptyImportService.ImportAllFilesAsync(context));
			}
		}
	}
}

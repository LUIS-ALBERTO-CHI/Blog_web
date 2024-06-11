using FwaEu.Fwamework.Configuration;
using FwaEu.Modules.ReportsProvidersByFileSystem;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.ReportsProvidersByFileSystem
{
	public class OptionsReportsProvidersByFileSystemOptionsStub : IOptions<ReportsProvidersByFileSystemOptions>
	{
		public ReportsProvidersByFileSystemOptions Value { get; }
			= new ReportsProvidersByFileSystemOptions()
			{
				StoragePaths = new[] { new PathEntry()
					{
						Path = "ReportsProvidersByFileSystem/Reports/*.report.json"
					}
				}
			};

		public ReportsProvidersByFileSystemOptions Get(string name)
		{
			throw new NotImplementedException();
		}

		public IDisposable OnChange(Action<ReportsProvidersByFileSystemOptions, string> listener)
		{
			throw new NotImplementedException();
		}
	}
}

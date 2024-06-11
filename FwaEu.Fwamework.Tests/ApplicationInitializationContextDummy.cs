using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests
{
	public class ApplicationInitializationContextDummy : ApplicationInitializationContext
	{
		public ApplicationInitializationContextDummy()
			: base(new ConfigurationDummy(), new WebHostEnvironmentDummy())
		{
			this.ServiceStore.Add<IRepositoryRegister>(new RepositoryRegister());
		}

		private class ConfigurationDummy : IConfiguration
		{
			public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

			public IEnumerable<IConfigurationSection> GetChildren()
			{
				throw new NotImplementedException();
			}

			public IChangeToken GetReloadToken()
			{
				throw new NotImplementedException();
			}

			public IConfigurationSection GetSection(string key)
			{
				throw new NotImplementedException();
			}
		}

		private class WebHostEnvironmentDummy : IWebHostEnvironment
		{
			public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public string WebRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
			public string EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		}
	}
}

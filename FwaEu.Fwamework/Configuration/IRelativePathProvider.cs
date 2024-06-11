using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Configuration
{
	public interface IRelativePathProvider
	{
		string RelativeTo { get; }
		string Path { get; }
	}

	public static class DefaultRelativePaths
	{
		public const string ApplicationRoot = nameof(ApplicationRoot);
	}

	public class ApplicationRootRelativePathProvider : IRelativePathProvider
	{
		public ApplicationRootRelativePathProvider(IHostEnvironment hostEnvironment)
		{
			this._hostEnvironment = hostEnvironment;
		}

		private readonly IHostEnvironment _hostEnvironment;

		public string RelativeTo => DefaultRelativePaths.ApplicationRoot;

		public string Path => this._hostEnvironment.ContentRootPath;
	}
}

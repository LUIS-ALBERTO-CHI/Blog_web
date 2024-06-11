using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FwaEu.Fwamework.Configuration
{
	public interface IRootPathProvider
	{
		string GetRootPath(PathEntry path);
	}

	public class DefaultRootPathProvider : IRootPathProvider
	{
		private readonly IRelativePathProvider[] _relativePathProviders;

		public DefaultRootPathProvider(IEnumerable<IRelativePathProvider> relativePathProviders)
		{
			this._relativePathProviders = relativePathProviders.ToArray();
		}

		private string GetRootPath(string pathRelativeTo)
		{
			if (String.IsNullOrEmpty(pathRelativeTo))
			{
				return null;
			}

			var provider = this._relativePathProviders.First(rpp => rpp.RelativeTo == pathRelativeTo);
			return provider.Path;
		}

		public string GetRootPath(PathEntry path)
		{
			var rootPath = this.GetRootPath(path.RelativeTo);
			return rootPath == null ? path.Path : Path.Combine(rootPath, path.Path);
		}
	}
}

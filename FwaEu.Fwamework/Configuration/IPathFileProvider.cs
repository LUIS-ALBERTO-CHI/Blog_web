using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Configuration
{
	public interface IPathFileProvider
	{
		IEnumerable<FileInfoByPathEntry> GetFiles(PathEntry[] paths);
	}

	public class FileInfoByPathEntry
	{
		public FileInfoByPathEntry(PathEntry pathEntry, FileInfo fileInfo,
			bool pathIsDirectory, string pathEntryAbsolutePath)
		{
			this.PathEntry = pathEntry ?? throw new ArgumentNullException(nameof(pathEntry));
			this.FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));

			this.PathEntryAbsolutePath = pathEntryAbsolutePath
				?? throw new ArgumentNullException(nameof(pathEntryAbsolutePath)); ;

			this.PathIsDirectory = pathIsDirectory;
		}

		public PathEntry PathEntry { get; }
		public FileInfo FileInfo { get; }
		public bool PathIsDirectory { get; }
		public string PathEntryAbsolutePath { get; }
	}

	public class DefaultPathFileProvider : IPathFileProvider
	{
		private readonly IRootPathProvider _rootPathProvider;

		public DefaultPathFileProvider(IRootPathProvider rootPathProvider)
		{
			this._rootPathProvider = rootPathProvider
				?? throw new ArgumentNullException(nameof(rootPathProvider));
		}

		public IEnumerable<FileInfoByPathEntry> GetFiles(PathEntry[] paths)
		{
			foreach (var path in paths)
			{
				var absolutePath = this._rootPathProvider.GetRootPath(path);

				var parts = absolutePath.Split('/', '\\').ToList();
				var lastPart = parts.Last();
				var hasWildcard = lastPart.Contains("*");
				var searchPattern = hasWildcard ? lastPart : "*"; //NOTE: The search pattern will be used on all subdirectories

				if (hasWildcard)
				{
					parts.RemoveAt(parts.Count - 1);
					absolutePath = String.Join('/', parts);
				}

				if (Directory.Exists(absolutePath))
				{
					foreach (var file in Directory.GetFiles(absolutePath, searchPattern, SearchOption.AllDirectories))
					{
						yield return new FileInfoByPathEntry(path,
							new FileInfo(file), true, Path.GetFullPath(absolutePath));
					}
				}
				else if (File.Exists(absolutePath))
				{
					yield return new FileInfoByPathEntry(path,
						new FileInfo(absolutePath), false, Path.GetFullPath(absolutePath));
				}
			}
		}
	}
}

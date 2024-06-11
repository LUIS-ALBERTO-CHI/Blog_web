using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Configuration
{
	public class CodeBasedRelativePathProvider : IRelativePathProvider
	{
		/// <param name="relativeTo">A key which will match with the PathEntry.RelativeTo when trying to resolve a path.</param>
		public CodeBasedRelativePathProvider(string relativeTo, string path)
		{
			this.RelativeTo = relativeTo ?? throw new ArgumentNullException(nameof(relativeTo));
			this.Path = path;
		}

		public string RelativeTo { get; }
		public string Path { get; }
	}
}

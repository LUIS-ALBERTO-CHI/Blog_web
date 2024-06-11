using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.FileTransactions
{
	public interface IStoragePathGenerator
	{
		string GetStorageRelativePath(string fileName);
	}
}

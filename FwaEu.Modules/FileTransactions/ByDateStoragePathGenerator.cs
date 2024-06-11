using FwaEu.Fwamework.Temporal;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.FileTransactions
{
	public class ByDateStoragePathGenerator : IStoragePathGenerator
	{
		/// <summary>
		/// {0} for year
		/// {1} for month
		/// </summary>
		public const string DirectoryMask = "{0:0000}-{1:00}/";

		public ByDateStoragePathGenerator(ICurrentDateTime currentDateTime)
		{
			this.CurrentDateTime = currentDateTime
				?? throw new ArgumentNullException(nameof(currentDateTime));
		}

		protected ICurrentDateTime CurrentDateTime { get; }

		public string GetStorageRelativePath(string fileName)
		{
			var storageFileName = Guid.NewGuid() + "_" + fileName;

			var date = this.CurrentDateTime.Now;
			var directory = String.Format(DirectoryMask, date.Year, date.Month);

			// NOTE: No need for Path.Combine() here because the directory is predictable (constant DirectoryMask)
			return directory + storageFileName;
		}
	}
}

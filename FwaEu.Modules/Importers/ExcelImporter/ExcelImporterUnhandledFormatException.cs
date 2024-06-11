using System;

namespace FwaEu.Modules.Importers.ExcelImporter
{
	public class ExcelImporterUnhandledFormatException : Exception
	{
		public ExcelImporterUnhandledFormatException(string message) : base(message)
		{
		}

		public ExcelImporterUnhandledFormatException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}

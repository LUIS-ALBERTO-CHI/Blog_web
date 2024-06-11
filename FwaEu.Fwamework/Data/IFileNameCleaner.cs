using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Data
{
	public interface IFileNameCleaner
	{
		string Clean(string fileName);
	}

	public class DefaultFileNameCleaner : IFileNameCleaner
	{
		public string Clean(string fileName)
		{
			//NOTE: Taken from https://stackoverflow.com/questions/309485/c-sharp-sanitize-file-name

			var invalidChars = System.Text.RegularExpressions.Regex.Escape(
				new string(System.IO.Path.GetInvalidFileNameChars()));

			var invalidRegexExpression = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

			return System.Text.RegularExpressions.Regex.Replace(fileName, invalidRegexExpression, "_");
		}
	}
}

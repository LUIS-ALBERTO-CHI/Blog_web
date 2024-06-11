using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Users
{
	public static class FormattingExtensions
	{
		private static readonly char[] Separators = { ' ', '-', '\'', '‘' };
		private static readonly string[] Particles = { "de", "du", "d", "l", "la", "le" };

		private static string FormatName(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return null;
			}
			value = value.Trim().ToLower();
			var hasSeparator = false;

			foreach (var separator in Separators)
			{
				if (value.Contains(separator))
				{
					var sb = new StringBuilder(value.Length);
					var firstPass = true;
					foreach (var part in value.Split(separator))
					{
						if (!firstPass)
						{
							sb.Append(separator);
						}
						sb.Append(Particles.Contains(part) ? part : FirstLetterUpperCase(part));
						firstPass = false;
					}
					value = sb.ToString();
					hasSeparator = true;
				}
			}
			if (!hasSeparator)
			{
				value = FirstLetterUpperCase(value);
			}
			return value;
		}

		private static string FirstLetterUpperCase(string part)
		{
			var hasSeparator = false;
			foreach (var separator in Separators)
				if (part.Contains(separator))
					hasSeparator = true;

			//TODO: Et si 0 caractères ? Et si un seul caractère ?
			return (hasSeparator ? part : part[0].ToString().ToUpper() + part.Substring(1));
		}

		public static string ToFullNameString(this IPerson person)
		{
			if (person == null)
			{
				throw new ArgumentNullException(nameof(person));
			}

			return $"{FormatName(person.FirstName)} {FormatName(person.LastName)}".Trim();
		}
	}
}

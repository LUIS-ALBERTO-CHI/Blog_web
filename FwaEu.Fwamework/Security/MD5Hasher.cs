using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Security
{
	public static class MD5Hasher
	{
		private static byte[] GetMD5Bytes(string value)
		{
			using (var md5 = MD5.Create())
			{
				return md5.ComputeHash(Encoding.UTF8.GetBytes(value));
			}
		}

		public static string ToMD5String(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(nameof(value));
			}

			var hash = GetMD5Bytes(value);
			var sb = new System.Text.StringBuilder();

			for (var i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("x2")); //NOTE: Hexadecimal format
			}

			return sb.ToString();
		}
	}
}

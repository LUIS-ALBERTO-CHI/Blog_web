using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Setup.ImportableFiles
{
	public interface IFileIdService
	{
		string GetFileId(string filePath);
	}

	public class Sha256HashFileIdService : IFileIdService
	{
		public string GetFileId(string filePath)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(filePath));
				return BitConverter.ToString(hashedBytes).Replace("-", "");
			}
		}
	}
}

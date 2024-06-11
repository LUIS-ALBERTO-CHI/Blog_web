using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public class Sha256PasswordHasher : IPasswordHasher
	{
		public Sha256PasswordHasher(IOptions<Sha256PasswordHasherOptions> settings)
		{
			this._settings = settings.Value;
		}

		private readonly Sha256PasswordHasherOptions _settings;

		public string Hash(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + this._settings.Salt));
				return BitConverter.ToString(hashedBytes).Replace("-", "");
			}
		}
	}

	public class Sha256PasswordHasherOptions
	{
		public string Salt { get; set; }
	}
}

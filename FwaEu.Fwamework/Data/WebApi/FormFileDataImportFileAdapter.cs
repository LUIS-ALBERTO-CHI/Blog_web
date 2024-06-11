using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Fwamework.Data.WebApi
{
	public class FormFileFileAdapter : IFile
	{
		public FormFileFileAdapter(IFormFile file)
		{
			this.File = file ?? throw new ArgumentNullException(nameof(file));
		}

		public IFormFile File { get; }

		public string Name => Path.GetFileName(this.File.FileName);//NOTE: Use Path.GetFileName because IE includes the full path on FileName property
		public long LengthInBytes => this.File.Length;

		public Stream OpenReadStream()
		{
			return this.File.OpenReadStream();
		}
	}
}

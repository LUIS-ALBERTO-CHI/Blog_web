using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Modules.Aspose
{
	public class AsposeLicense
	{
		public Type Type { get; }

		public AsposeLicense(Type type)
		{
			this.Type = type ?? throw new ArgumentNullException(nameof(type));
		}

		public void SetLicense()
		{
			var instance = Activator.CreateInstance(this.Type);

			lock (this.Type)
			{
				this.Type.GetMethod("SetLicense", new[] { typeof(string) })
					.Invoke(instance, new object[] { "Aspose.Total.NET.lic" });
			}
		}
	}
}

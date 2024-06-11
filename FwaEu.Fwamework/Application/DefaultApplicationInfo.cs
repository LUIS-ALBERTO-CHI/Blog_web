using FwaEu.Fwamework.Temporal;
using Microsoft.Extensions.Options;
using System;

namespace FwaEu.Fwamework.Application
{
	public class DefaultApplicationInfo<TStartup> : IApplicationInfo
		where TStartup : class
	{
		public DefaultApplicationInfo(IOptions<ApplicationInfoOptions> settings,
			ICurrentDateTime currentDateTime)
		{
			_ = settings ?? throw new ArgumentNullException(nameof(settings));

			var assemblyName = typeof(TStartup).Assembly.GetName();
			this.Version = settings.Value?.Version ?? assemblyName.Version.ToString(3);
			this.Name = settings.Value?.DisplayName ?? assemblyName.Name;
			this.CopyrightYears = settings.Value?.CopyrightYears;
			if (this.CopyrightYears != null && this.CopyrightYears.Contains("{0}"))
			{
				this.CopyrightYears  = String.Format(this.CopyrightYears, currentDateTime.Now.Year);
			}
		}

		public string Version { get; }
		public string Name { get; }

		public string CopyrightYears { get; }
	}
}

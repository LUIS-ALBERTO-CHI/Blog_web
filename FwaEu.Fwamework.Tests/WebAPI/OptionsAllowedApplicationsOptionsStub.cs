using FwaEu.Fwamework.WebApi;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.WebAPI
{
	public class OptionsAllowedApplicationsOptionsStub : IOptions<AllowedApplicationsOptions>
	{
		public AllowedApplicationsOptions Value => new AllowedApplicationsOptions()
		{
			Applications = new(){
				{ "Roberto", new ApplicationEntry
					{
						Secret = "Martin",
						Filter =
							new Filter
							{
								 Allowed = new [] { "0.0.0.1", "99.99.99.99" }
							}
					}
				}
			}
		};

		public AllowedApplicationsOptions Get(string name)
		{
			throw new NotImplementedException();
		}

		public IDisposable OnChange(Action<AllowedApplicationsOptions, string> listener)
		{
			throw new NotImplementedException();
		}
	}

	public class OptionsAllowedApplicationsOptionsNoFilterStub : IOptions<AllowedApplicationsOptions>
	{
		public AllowedApplicationsOptions Value => new AllowedApplicationsOptions()
		{
			Applications = new(){
				{ "Roberto", new ApplicationEntry
					{
						Secret = "Martin",
						Filter = null
					}
				}
			}
		};

		public AllowedApplicationsOptions Get(string name)
		{
			throw new NotImplementedException();
		}

		public IDisposable OnChange(Action<AllowedApplicationsOptions, string> listener)
		{
			throw new NotImplementedException();
		}
	}

	public class OptionsAllowedApplicationsOptionsEmptyFilterStub : IOptions<AllowedApplicationsOptions>
	{
		public AllowedApplicationsOptions Value => new AllowedApplicationsOptions()
		{
			Applications = new(){
				{ "Roberto", new ApplicationEntry
					{
						Secret = "Martin",
						Filter =
							new Filter
							{
									Allowed = new string[]{ }
							}
					}
				}
			}
		};

		public AllowedApplicationsOptions Get(string name)
		{
			throw new NotImplementedException();
		}

		public IDisposable OnChange(Action<AllowedApplicationsOptions, string> listener)
		{
			throw new NotImplementedException();
		}
	}
}

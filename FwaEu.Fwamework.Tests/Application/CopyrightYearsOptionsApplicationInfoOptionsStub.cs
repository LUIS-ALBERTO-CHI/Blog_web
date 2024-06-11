using FwaEu.Fwamework.Application;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Fwamework.Tests.Application
{
	public class CopyrightYearsOptionsApplicationInfoOptionsStub : IOptions<ApplicationInfoOptions>
	{
		public CopyrightYearsOptionsApplicationInfoOptionsStub(string copyrightYears)
		{
			this.Value = new ApplicationInfoOptions()
			{
				Version = null,
				DisplayName = null,
				CopyrightYears = copyrightYears,
			};
		}

		public ApplicationInfoOptions Value { get; }
	}
}

using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Data.Database
{
	public static class DatabaseExceptionHelper
	{
		public static void CheckForDbConstraints(GenericADOException exception)
		{
			var fwameworkException = ApplicationServices.ServiceProvider
				.GetServices<IDatabaseFeaturesProvider>()
				.Select(dfp => dfp.GetDatabaseFeatures().CreateException(exception))
				.FirstOrDefault(ex => ex != null);

			if (fwameworkException != null)
			{
				throw fwameworkException;
			}
		}
	}
}

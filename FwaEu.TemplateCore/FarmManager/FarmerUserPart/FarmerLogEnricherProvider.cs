using FwaEu.Fwamework.Users;
using FwaEu.Modules.Logging;
using System;
using System.Collections.Generic;

namespace FwaEu.TemplateCore.FarmManager.FarmerUserPart
{
	public class FarmerLogEnricherProvider : ILogEnricherProvider
	{
		private readonly ICurrentUserService _currentUserService;

		public FarmerLogEnricherProvider(ICurrentUserService currentUserService)
		{
			this._currentUserService = currentUserService
				?? throw new ArgumentNullException(nameof(currentUserService));
		}

		public IEnumerable<LogEnricherProperty> GetProperties()
		{
			if (this._currentUserService.User.Entity is
				IFarmerPartEntityPropertiesAccessor farmerUserProperties)
			{
				yield return new LogEnricherProperty("FarmerPseudonym",
					farmerUserProperties.FarmerPseudonym, false);

				yield return new LogEnricherProperty("FarmerDestructureSample",
					new
					{
						OwnsHisLand = true,
						AnimalCount = 100
					}, true);
			}
		}
	}
}

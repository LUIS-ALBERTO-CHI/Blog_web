using FwaEu.BlogWeb.FarmManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.ViewContext
{
	public class ViewContextModel
	{
		public TownRegionEntity Region { get; }
		public ViewContextModel(TownRegionEntity entity)
		{
			this.Region= entity;
		}

		/// <summary>
		/// Used by logs and also to convert view context to reporting parameters.
		/// </summary>
		public Dictionary<string, object> ToDictionary()
		{
			return new Dictionary<string, object>()
			{
				 { "RegionId", this.Region?.Id  } 
			};
		}
	}
}

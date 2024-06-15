using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.ViewContext.WebApi
{
	public class ViewContextApiModel
	{
		//NOTE: No [Required] here because the UI will accept to select no region ("All regions")
		public int? RegionId { get; set; }
	}
}

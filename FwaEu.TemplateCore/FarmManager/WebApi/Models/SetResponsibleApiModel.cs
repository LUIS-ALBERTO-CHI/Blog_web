using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.WebApi.Models
{
	public class SetResponsibleApiModel
	{
		public SetResponsibleApiModel(int? responsibleId)
		{
			ResponsibleId = responsibleId;
		}

		public int? ResponsibleId { get; }
	}
}

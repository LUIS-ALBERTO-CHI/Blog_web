using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.WebApi.Models
{
	public class FarmSaveResultApiModel
	{
		public FarmSaveResultApiModel(int id)
		{
			Id = id;
		}

		public int Id { get; }
	}
}

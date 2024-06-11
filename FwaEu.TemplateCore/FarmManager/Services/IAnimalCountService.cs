using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.TemplateCore.FarmManager.Models;

namespace FwaEu.TemplateCore.FarmManager.Services
{
	public interface IAnimalCountService
	{
		Task<IEnumerable<AnimalCountBySpeciesModel>> GetAllAsync(int farmId);
		Task SaveOrDeleteAsync(int farmId, AnimalCountSaveModel[] models);
	}
}

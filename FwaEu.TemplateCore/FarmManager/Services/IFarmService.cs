using FwaEu.TemplateCore.FarmManager.Models;
using FwaEu.TemplateCore.FarmManager.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.Services
{
	public interface IFarmService
	{
		Task<IEnumerable<FarmListModel>> GetAllAsync(bool onlyFarmsWithoutAnimals);
		Task<FarmGeneralInformationModel> GetGeneralInformationAsync(int id);
		Task<int> SaveAsync(FarmGeneralInformationModel model);
		Task DeleteAsync(int id);
	}
}

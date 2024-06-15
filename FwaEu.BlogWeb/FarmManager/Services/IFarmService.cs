using FwaEu.BlogWeb.FarmManager.Models;
using FwaEu.BlogWeb.FarmManager.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Services
{
	public interface IFarmService
	{
		Task<IEnumerable<FarmListModel>> GetAllAsync(bool onlyFarmsWithoutAnimals);
		Task<FarmGeneralInformationModel> GetGeneralInformationAsync(int id);
		Task<int> SaveAsync(FarmGeneralInformationModel model);
		Task DeleteAsync(int id);
	}
}

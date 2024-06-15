using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.ReportsUserViewsByEntities;
using FwaEu.BlogWeb.FarmManager.Entities;
using FwaEu.BlogWeb.FarmManager.Models;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Services
{
	public class DefaultAnimalCountService : IAnimalCountService
	{
		public DefaultAnimalCountService(IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory)
		{
			_repositorySessionFactory = repositorySessionFactory
				?? throw new ArgumentNullException(nameof(repositorySessionFactory));
		}

		private readonly IRepositorySessionFactory<IStatefulSessionAdapter> _repositorySessionFactory;

		public async Task<IEnumerable<AnimalCountBySpeciesModel>> GetAllAsync(int farmId)
		{
			using (var repositorySession = _repositorySessionFactory.CreateSession())
			{
				var models = await repositorySession
					.Create<FarmAnimalCountEntityRepository>()
					.Query()
					.Where(entity => entity.Farm.Id == farmId)
					.Select(entity => new AnimalCountBySpeciesModel(
						entity.Quantity, entity.Species.Id,
						entity.UpdatedOn, entity.UpdatedBy.Id))
					.ToListAsync();

				return models;
			}
		}

		public async Task SaveOrDeleteAsync(int farmId, AnimalCountSaveModel[] models)
		{
			using (var repositorySession = _repositorySessionFactory.CreateSession())
			{
				using (var transaction = repositorySession.Session.BeginTransaction())
				{

					var farm = await repositorySession.GetOrNotFoundExceptionAsync<FarmEntity, int, FarmEntityRepository>(farmId);

					var repository = repositorySession
						.Create<FarmAnimalCountEntityRepository>();

					var animalSpeciesRepository = repositorySession
						.Create<FarmAnimalSpeciesEntityRepository>();

					var farmSpeciesIds = models.Select(m => m.AnimalSpeciesId).ToArray();

					var existingBySpeciesId = (await repository.Query()
						.Where(fas => fas.Farm == farm && farmSpeciesIds.Contains(fas.Species.Id))
						.Fetch(fas => fas.Species)
						.ToListAsync())
						.ToDictionary(fas => fas.Species.Id);

					foreach (var model in models)
					{
						var entity = existingBySpeciesId.ContainsKey(model.AnimalSpeciesId)
							? existingBySpeciesId[model.AnimalSpeciesId]
							: new FarmAnimalCountEntity()
							{
								Farm = farm,
								Species = model.Quantity == 0 // NOTE: Optimization, no need to load the master-data when we will not create anything
									? null : await animalSpeciesRepository
										.GetAsync(model.AnimalSpeciesId)
							};

						entity.Quantity = model.Quantity;

						if (entity.Quantity == 0)
						{
							if (!entity.IsNew())
							{
								await repository.DeleteAsync(entity);
							}
						}
						else
						{
							await repository.SaveOrUpdateAsync(entity);
						}
					}

					await transaction.CommitAsync();
					await repositorySession.Session.FlushAsync();
				}
			}
		}
	}
}

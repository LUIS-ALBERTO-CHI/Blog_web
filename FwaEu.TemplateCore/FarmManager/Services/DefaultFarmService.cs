using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.Data.Database;
using FwaEu.TemplateCore.FarmManager.Entities;
using FwaEu.TemplateCore.FarmManager.Models;
using NHibernate.Exceptions;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.TemplateCore.FarmManager.Services
{
	public class DefaultFarmService : IFarmService
	{
		public DefaultFarmService(MainSessionContext sessionContext)
		{
			_sessionContext = sessionContext
				?? throw new ArgumentNullException(nameof(sessionContext));
		}

		private readonly MainSessionContext _sessionContext;

		public async Task<IEnumerable<FarmListModel>> GetAllAsync(bool onlyFarmsWithoutAnimals)
		{
			var repositorySession = _sessionContext.RepositorySession;

			var animalCountQuery = repositorySession
				.Create<FarmAnimalCountEntityRepository>()
				.Query();

			var getFarmsQuery = repositorySession
				.Create<FarmEntityRepository>()
				.Query()
				.Select(x => new FarmListModel
				{
					Id = x.Id,
					Name = x.Name,
					PostalCodeId = x.PostalCode.Id,
					RegionId = x.PostalCode.Town.Region.Id,
					CategorySize = x.CategorySize,
					MainActivityId = x.MainActivity.Id,
					SellingPriceInEurosWithoutTaxes = x.SellingPriceInEurosWithoutTaxes,
					RecruitEmployees = x.RecruitEmployees,
					OpeningDate = x.OpeningDate,
					ClosingDate = x.ClosingDate,
					AnimalCount = (int?)animalCountQuery
						.Where(ac => ac.Farm == x)
						.Sum(ac => ac.Quantity) ?? 0,
					UpdatedOn = x.UpdatedOn,
					UpdatedById = x.UpdatedBy.Id
				});

			if (onlyFarmsWithoutAnimals)
				getFarmsQuery = getFarmsQuery.Where(x => x.AnimalCount == 0);

			var farmModels = await getFarmsQuery

				.ToListAsync();

			return farmModels;
		}

		public async Task<FarmGeneralInformationModel> GetGeneralInformationAsync(int id)
		{
			var repositorySession = _sessionContext.RepositorySession;

			var model = await repositorySession
					.Create<FarmEntityRepository>()
					.Query()
					.Where(entity => entity.Id == id)
					.Select(entity => new FarmGeneralInformationModel
					{
						Id = entity.Id,
						Name = entity.Name,
						PostalCodeId = entity.PostalCode.Id,
						CategorySize = entity.CategorySize,
						MainActivityId = entity.MainActivity.Id,
						SellingPriceInEurosWithoutTaxes = entity.SellingPriceInEurosWithoutTaxes,
						RecruitEmployees = entity.RecruitEmployees,
						OpeningDate = entity.OpeningDate,
						ClosingDate = entity.ClosingDate,
						Comments = entity.Comments,
						CreatedOn = entity.CreatedOn,
						CreatedById = entity.CreatedBy.Id,
						UpdatedOn = entity.UpdatedOn,
						UpdatedById = entity.UpdatedBy.Id
					})
					.SingleOrDefaultAsync();

			return model;
		}

		public async Task<int> SaveAsync(FarmGeneralInformationModel model)
		{
			try
			{
				var repositorySession = _sessionContext.RepositorySession;

				var repository = repositorySession
						.Create<FarmEntityRepository>();

				var entity = model.Id == 0
					? new FarmEntity()
					: await LoadFarmAsync(model.Id, repository);

				entity.PostalCode = await repositorySession
					.Create<FarmPostalCodeEntityRepository>()
					.GetAsync(model.PostalCodeId); //TODO: ?? throw ReferenceException https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/7139/

				entity.MainActivity = await repositorySession
					.Create<FarmActivityEntityRepository>()
					.GetAsync(model.MainActivityId); //TODO: ?? throw ReferenceException https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/7139/

				entity.Name = model.Name;
				entity.CategorySize = model.CategorySize;
				entity.SellingPriceInEurosWithoutTaxes = model.SellingPriceInEurosWithoutTaxes;
				entity.RecruitEmployees = model.RecruitEmployees;
				entity.OpeningDate = model.OpeningDate;
				entity.ClosingDate = model.ClosingDate;
				entity.Comments = model.Comments;

				await repository.SaveOrUpdateAsync(entity);
				await repositorySession.Session.FlushAsync();

				return entity.Id;
			}
			catch (GenericADOException e)
			{
				DatabaseExceptionHelper.CheckForDbConstraints(e);
				throw;
			}
		}

		public async Task DeleteAsync(int id)
		{
			var repositorySession = _sessionContext.RepositorySession;

			using (var transaction = repositorySession.Session.BeginTransaction())
			{
				var repository = repositorySession
					.Create<FarmEntityRepository>();

				var entity = await LoadFarmAsync(id, repository);

				await repositorySession
					.Create<FarmAnimalCountEntityRepository>()
					.Query().Where(ac => ac.Farm == entity)
					.DeleteAsync(CancellationToken.None);

				await repository.DeleteAsync(entity);
				await transaction.CommitAsync();
				await repositorySession.Session.FlushAsync();
			}
		}

		public static async Task<FarmEntity> LoadFarmAsync(int id, FarmEntityRepository repository)
		{
			var entity = await repository.GetAsync(id);

			if (entity == null)
			{
				throw new NotFoundException($"Farm not found with id #{id}.");
			}

			return entity;
		}
	}
}

using FwaEu.Modules.GenericAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	public enum DonkeyGender
	{
		Male,
		Female,
		Fluid,
		Other
	}

	public class ModelAttributeMockGenericAdminModelConfiguration
		: ModelAttributeGenericAdminModelConfiguration<ModelAttributeMockModel>
	{
		private static class DataSource // NOTE: Could be a db repository, a webservice call, an XML serialization on the file system...
		{
			private static List<ModelAttributeMockModel> Models = new List<ModelAttributeMockModel>()
			{
				new ModelAttributeMockModel() { Id = 1, Name = "Robert", CityId = 1, Gender = DonkeyGender.Male },
				new ModelAttributeMockModel() { Id = 2, Name = "Martin", CityId = 2, Gender = DonkeyGender.Other },
				new ModelAttributeMockModel() { Id = 3, Name = "Géraldine", CityId = 2, Gender = DonkeyGender.Female },
				new ModelAttributeMockModel() { Id = 4, Name = "Pénélope", CityId = 1, Gender = DonkeyGender.Fluid },
			};

			public static IEnumerable<ModelAttributeMockModel> GetAll()
			{
				return Models.ToArray(); //NOTE: ToArray because we need a copy of the enumerable, not the reference of the current state
			}

			public static ModelAttributeMockModel Save(ModelAttributeMockModel model)
			{
				var wasNew = model.Id == null;
				var saveModel = model.Id == null ? model
					: Models.First(m => m.Id.Value == model.Id.Value);

				if (wasNew)
				{
					saveModel.Id = Models.Max(m => m.Id) + 1;
					Models.Add(saveModel);
				}
				else
				{
					saveModel.Name = model.Name;
					saveModel.CityId = model.CityId;
					saveModel.Gender = model.Gender;
				}

				return saveModel;
			}

			public static void Delete(int id)
			{
				Models.Remove(Models.First(m => m.Id == id));
			}
		}

		public const string Key = "MockByModelAttribute";

		private AuthorizedActions _authorizedActions;

		public ModelAttributeMockGenericAdminModelConfiguration(IServiceProvider serviceProvider, AuthorizedActions authorizedActions = null)
			: base(serviceProvider)
		{
			this._authorizedActions = authorizedActions;
		}

		public override Task<LoadDataResult<ModelAttributeMockModel>> GetModelsAsync(GenericAdminGetModelsParameters parameters)
		{
			if (this.GettingModels != null)
			{
				this.GettingModels();
			}

			return Task.FromResult(new LoadDataResult<ModelAttributeMockModel>(
				new ListDataSource<ModelAttributeMockModel>(DataSource.GetAll().ToList(), null)));
		}

		public event GettingModels GettingModels; //HACK: For unit tests

		protected override void OnPropertyCreated(Property property)
		{
			base.OnPropertyCreated(property);

			if (property.Name == nameof(ModelAttributeMockModel.CityId))
			{
				property.ExtendedProperties.Add("CustomProperty", "Blabla");
			}
		}

		protected override Task<ModelAttributeMockModel> SaveModelAsync(ModelAttributeMockModel model)
		{
			return Task.FromResult(DataSource.Save(model));
		}

		protected override Task<SimpleDeleteModelResult> DeleteModelAsync(ModelAttributeMockModel model)
		{
			DataSource.Delete(model.Id.Value);
			return Task.FromResult(new SimpleDeleteModelResult());
		}

		protected override bool IsNew(ModelAttributeMockModel model)
		{
			return model.Id == null;
		}

		public override Task<bool> IsAccessibleAsync()
		{
			return Task.FromResult(true);
		}

		public override AuthorizedActions GetAuthorizedActions()
		{
			if (this._authorizedActions != null)
			{
				return this._authorizedActions;
			}
			return base.GetAuthorizedActions();
		}
	}

	public delegate void GettingModels();
}

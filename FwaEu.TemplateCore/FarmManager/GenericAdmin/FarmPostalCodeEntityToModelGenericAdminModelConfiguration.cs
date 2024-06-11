using FwaEu.Modules.GenericAdmin;
using System.ComponentModel.DataAnnotations;
using System;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FwaEu.Modules.GenericAdminMasterData;
using FwaEu.Fwamework.Globalization;
using FwaEu.TemplateCore.FarmManager.Entities;


namespace FwaEu.TemplateCore.FarmManager.GenericAdmin
{
	public class FarmPostalCodeModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }
		[Required]
		public string PostalCode { get; set; }
		[Required]
		[MasterData("FarmTowns")]
		public int? TownId { get; set; }
		[Property(IsEditable = false)]
		public DateTime? CreatedOn { get; set; }
		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? CreatedById { get; set; }
		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }
		[Property(IsEditable = false)]
		[MasterData("Users")]
		public int? UpdatedById { get; set; }
	}
	public class FarmPostalCodeEntityToModelGenericAdminModelConfiguration
			: EntityToModelGenericAdminModelConfiguration<FarmPostalCodeEntity, int, FarmPostalCodeModel, MainSessionContext>
	{
		public const string Key = nameof(FarmPostalCodeEntity);

		private readonly CurrentUserPermissionService _currentUserPermissionService;
		public FarmPostalCodeEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService,
			CurrentUserPermissionService currentUserPermissionService)
			: base(serviceProvider, culturesService)
		{
			_currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		public override async Task<bool> IsAccessibleAsync()
		{
			return await _currentUserPermissionService.HasPermissionAsync<FarmManagerPermissionProvider>(
				provider => provider.CanAdministrateFarmMasterData);
		}

		protected override async Task FillEntityAsync(FarmPostalCodeEntity entity, FarmPostalCodeModel model)
		{
			var session = RepositorySession.Session;

			entity.PostalCode = model.PostalCode;
			entity.Town = await session.GetAsync<FarmTownEntity>(model.TownId);

		}

		protected override async Task<FarmPostalCodeEntity> GetEntityAsync(FarmPostalCodeModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<FarmPostalCodeEntity, FarmPostalCodeModel>> GetSelectExpression()
		{
			return entity => new FarmPostalCodeModel
			{
				Id = entity.Id,
				TownId = entity.Town.Id,
				PostalCode = entity.PostalCode,
				UpdatedOn = entity.UpdatedOn,
				UpdatedById = entity.UpdatedBy.Id,
				CreatedById = entity.CreatedBy.Id,
				CreatedOn = entity.CreatedOn,
			};
		}

		protected override bool IsNew(FarmPostalCodeModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(FarmPostalCodeModel model, FarmPostalCodeEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}

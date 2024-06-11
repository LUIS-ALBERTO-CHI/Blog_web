using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericAdmin;
using NHibernate.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies.GenericAdmin
{
	public class CurrencyModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		[LocalizableStringCustomType]
		public Dictionary<string, string> Name { get; set; }

		[Required]
		public bool? IsInverse { get; set; }

		[Required]
		public string CurrencyCode { get; set; }

		[Required]
		public int? Precision { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }
	}

	public class CurrencyEntityToModelGenericAdminModelConfiguration
		: EntityToModelGenericAdminModelConfiguration<CurrencyEntity, int, CurrencyModel, MainSessionContext>
	{
		public const string Key = nameof(CurrencyEntity);

		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public CurrencyEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService,
			CurrentUserPermissionService currentUserPermissionService)
			: base(serviceProvider, culturesService)
		{
			_currentUserPermissionService = currentUserPermissionService 
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		public override async Task<bool> IsAccessibleAsync()
		{
			return await _currentUserPermissionService.HasPermissionAsync<CurrenciesPermissionProvider>(
				provider => provider.CanAdministrateCurrencies);
		}

		protected override Task FillEntityAsync(CurrencyEntity entity, CurrencyModel model)
		{
			entity.Name = model.Name.ToStringStringDictionary();
			entity.CurrencyCode = model.CurrencyCode;
			entity.IsInverse = model.IsInverse.Value;
			entity.Precision = model.Precision.Value;

			return Task.CompletedTask;
		}

		protected override async Task<CurrencyEntity> GetEntityAsync(CurrencyModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<CurrencyEntity, CurrencyModel>> GetSelectExpression()
		{
			return entity => new CurrencyModel
			{
				Id = entity.Id,
				Name = entity.Name.ToStringStringDictionary(),
				IsInverse = entity.IsInverse,
				CurrencyCode = entity.CurrencyCode,
				Precision = entity.Precision,
				UpdatedOn = entity.UpdatedOn,
			};
		}

		protected override bool IsNew(CurrencyModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(CurrencyModel model, CurrencyEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}

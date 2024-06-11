using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.Currencies.MasterData;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.Currencies.GenericAdmin
{
	public class ExchangeRateModel
	{
		[Property(IsKey = true, IsEditable = false)]
		public int? Id { get; set; }

		[Required]
		public bool? IsInverse { get; set; }

		[Required]
		[MasterData("Currencies")]
		public string BaseCurrencyCode { get; set; }

		[Required]
		[MasterData("Currencies")]
		public string QuotedCurrencyCode { get; set; }

		[Required]
		public decimal? Value { get; set; }

		public DateTime? Date { get; set; }

		[Property(IsEditable = false)]
		public DateTime? UpdatedOn { get; set; }
	}

	public class ExchangeRateEntityToModelGenericAdminModelConfiguration 
		: EntityToModelGenericAdminModelConfiguration<ExchangeRateEntity, int, ExchangeRateModel, MainSessionContext>
	{
		public const string Key = nameof(ExchangeRateEntity);

		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public ExchangeRateEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService,
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

		protected override Task FillEntityAsync(ExchangeRateEntity entity, ExchangeRateModel model)
		{
			entity.BaseCurrencyCode = model.BaseCurrencyCode;
			entity.QuotedCurrencyCode = model.QuotedCurrencyCode;
			entity.IsInverse = model.IsInverse.Value;
			entity.Value = model.Value.Value;
			entity.Date = model.Date;
			return Task.CompletedTask;
		}

		protected override async Task<ExchangeRateEntity> GetEntityAsync(ExchangeRateModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<ExchangeRateEntity, ExchangeRateModel>> GetSelectExpression()
		{
			return entity => new ExchangeRateModel
			{
				Id = entity.Id,
				IsInverse = entity.IsInverse,
				BaseCurrencyCode = entity.BaseCurrencyCode,
				QuotedCurrencyCode = entity.QuotedCurrencyCode,
				Value = entity.Value,
				Date = entity.Date,
				UpdatedOn = entity.UpdatedOn,
			};
		}

		protected override bool IsNew(ExchangeRateModel model)
		{
			return model.Id == null;
		}

		protected override void SetIdToModel(ExchangeRateModel model, ExchangeRateEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}

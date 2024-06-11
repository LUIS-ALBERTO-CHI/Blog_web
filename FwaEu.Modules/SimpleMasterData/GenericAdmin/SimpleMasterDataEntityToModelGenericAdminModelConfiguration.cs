using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.GenericAdmin;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Modules.SimpleMasterData.GenericAdmin
{
	public class SimpleMasterDataEntityToModelGenericAdminModelConfiguration<TEntity, TModel>
		: SimpleMasterDataEntityToModelGenericAdminModelConfiguration<TEntity, TModel, MainSessionContext>
		where TEntity : SimpleMasterDataEntityBase, new()
		where TModel : SimpleMasterDataGenericAdminModel, new()
	{
		public SimpleMasterDataEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider, ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{
		}
	}

	public class SimpleMasterDataEntityToModelGenericAdminModelConfiguration<TEntity, TModel, TSessionContext>
		: EntityToModelGenericAdminModelConfiguration<TEntity, int, TModel, TSessionContext>
		where TEntity : SimpleMasterDataEntityBase, new()
		where TModel : SimpleMasterDataGenericAdminModel, new()
		where TSessionContext : BaseSessionContext<IStatefulSessionAdapter>
	{
		public SimpleMasterDataEntityToModelGenericAdminModelConfiguration(
			IServiceProvider serviceProvider,
			ICulturesService culturesService)
			: base(serviceProvider, culturesService)
		{
		}

		public override async Task<bool> IsAccessibleAsync()
		{
			var securityManagerFactory = this.ServiceProvider
				.GetService<IGenericAdminSimpleMasterDataSecurityManagerFactory<TEntity>>();

			if (securityManagerFactory == null)
			{
				return true;
			}

			return await securityManagerFactory
				.CreateManager(this.ServiceProvider)
				.IsAccessibleAsync();
		}

		protected override Task FillEntityAsync(TEntity entity, TModel model)
		{
			entity.InvariantId = model.InvariantId;
			entity.Name = model.Name.ToStringStringDictionary();

			return Task.CompletedTask;
		}

		protected override async Task<TEntity> GetEntityAsync(TModel model)
		{
			return await this.GetRepository().GetAsync(model.Id.Value);
		}

		protected override Expression<Func<TEntity, TModel>> GetSelectExpression()
		{
			return entity => new TModel
			{
				Id = entity.Id,
				InvariantId = entity.InvariantId,
				Name = entity.Name.ToStringStringDictionary(),
				UpdatedOn = entity.UpdatedOn,
				UpdatedById = entity.UpdatedBy.Id,
				CreatedById = entity.CreatedBy.Id,
				CreatedOn = entity.CreatedOn,
			};
		}

		protected override bool IsNew(TModel model)
		{
			return !model.Id.HasValue;
		}

		protected override void SetIdToModel(TModel model, TEntity entity)
		{
			model.Id = entity.Id;
		}
	}
}

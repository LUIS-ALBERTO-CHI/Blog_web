using FwaEu.Fwamework;
using FwaEu.Fwamework.Formatting;
using FwaEu.Fwamework.Imports;
using FwaEu.Fwamework.ProcessResults;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	public class ModelImporterContext
	{
		public ModelImporterContext(ProcessResult processResult, ServiceStore serviceStore)
		{
			this.ProcessResult = processResult ?? throw new ArgumentNullException(nameof(processResult));
			this.ServiceStore = serviceStore ?? throw new ArgumentNullException(nameof(serviceStore));
		}

		public ProcessResult ProcessResult { get; }
		public ServiceStore ServiceStore { get; }
	}

	public interface IModelImporter
	{
		Task ImportAsync(DataReader reader, ModelImporterContext context);
	}

	public interface IModelImporter<TModel> : IModelImporter
		where TModel : class
	{
	}

	public class ModelLoadResult<TModel>
		where TModel : class
	{
		public ModelLoadResult(TModel model, bool isNew)
		{
			this.Model = model ?? throw new ArgumentNullException(nameof(model));
			this.IsNew = isNew;
		}

		public TModel Model { get; }
		public bool IsNew { get; }
	}

	public abstract class ModelImporter<TModel, TEventListener> : IModelImporter<TModel>
		where TModel : class
		where TEventListener : IModelImporterEventListener
	{
		protected ModelImporter(IServiceProvider serviceProvider, IModelBinder<TModel> modelBinder)
		{
			this.ServiceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));

			this.ModelBinder = modelBinder
				?? throw new ArgumentNullException(nameof(modelBinder));

			this._toStringService = new Lazy<IToStringService>(
				() => this.ServiceProvider.GetRequiredService<IToStringService>());
		}

		private Lazy<IToStringService> _toStringService;
		public IServiceProvider ServiceProvider { get; }
		public IModelBinder<TModel> ModelBinder { get; }

		protected virtual ModelImporterEventListeners<TEventListener> CreateEventListener(IServiceProvider serviceProvider, ServiceStore serviceStore)
		{
			
			var factories = serviceProvider.GetServices<IModelImporterEventListenerFactory<TEventListener>>();

			return new ModelImporterEventListeners<TEventListener>(factories.Select(x => x.Create(serviceStore, serviceProvider)).ToArray());
		}

		protected abstract Task<ModelLoadResult<TModel>> LoadModelAsync(
			ModelPropertyDescriptor[] keys, DataRow row, ModelImporterContext context);

		protected abstract Task SaveModelAsync(
			TModel model, ModelImporterContext context);

		protected virtual ProcessResultEntry CreateProcessResultEntry(ModelLoadResult<TModel> modelLoadResult)
		{
			var modelAsString = this._toStringService.Value.ToString(
				modelLoadResult.Model, null, CultureInfo.InvariantCulture);

			if (modelLoadResult.IsNew)
			{
				return new ModelCreatedProcessResultEntry(modelAsString);
			}

			return new ModelUpdatedProcessResultEntry(modelAsString);
		}

		protected virtual async Task ImportRowAsync(
			ModelPropertyDescriptor[] keys, DataRow row,
			ModelImporterContext context, ProcessResultContext processResultContext,
			ModelImporterEventListeners<TEventListener> eventListener)
		{
			var modelLoadResult = await this.LoadModelAsync(keys, row, context);

			await this.ModelBinder.BindToModelAsync(modelLoadResult.Model, row, context.ServiceStore);

			var model = modelLoadResult.Model;

			await eventListener.OnModelSavingAsync(model);
			await this.SaveModelAsync(model, context);
			await eventListener.OnModelSavedAsync(model);

			processResultContext.Add(this.CreateProcessResultEntry(modelLoadResult));
		}

		protected virtual ProcessResultContext GetProcessResultContext(ModelImporterContext context)
		{
			return context.ProcessResult.CreateContext($"Import of {typeof(TModel).Name}",
				"ModelImporter", new { ModelName = typeof(TModel).Name });
		}
		protected virtual async Task ImportAsync(DataReader reader, ModelImporterContext context, ProcessResultContext processResultContext)
		{
			var keys = reader.GetProperties()
				.Where(p => p.IsKey.HasFlag(IsKeyValue.True))
			.ToArray();

			await using (var eventListener = this.CreateEventListener(this.ServiceProvider, context.ServiceStore))
			{
				foreach (var row in reader.GetRows())
				{
					try
					{
						await this.ImportRowAsync(keys, row, context, processResultContext, eventListener);
					}
					catch (Exception ex)
					{
						processResultContext.Add(ErrorProcessResultEntry.FromException(ex));
						await eventListener.OnModelSavingErrorAsync(ex);
						continue;
					}
				}
				await eventListener.OnImportFinished();
			}
		}

		public async Task ImportAsync(DataReader reader, ModelImporterContext context)
		{
			var processResultContext = this.GetProcessResultContext(context);
			await this.ImportAsync(reader, context, processResultContext);
		}
	}
}

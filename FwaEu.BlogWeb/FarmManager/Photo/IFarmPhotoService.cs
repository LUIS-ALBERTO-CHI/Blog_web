using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.FileTransactions;
using FwaEu.BlogWeb.FarmManager.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.BlogWeb.FarmManager.Photo
{
	public interface IFarmPhotoService
	{
		Task SavePhotoAsync(int farmId, IFile photo);
		Task<InMemoryFile> GetPhotoAsync(int farmId); // NOTE: Returns InMemoryFile and not IFile to ensure that bytes are loaded (e.g. implementation using database will close the session)

		/// <summary>
		/// Delete the photo if exists.
		/// </summary>
		/// <returns>
		/// true is photo was existing, and then deleted.
		/// false is photo was not existing.
		/// </returns>
		Task<bool> DeletePhotoAsync(int farmId);
	}

	public class DefaultFarmPhotoService : IFarmPhotoService
	{
		private readonly IRepositorySessionFactory<IStatefulSessionAdapter> _repositorySessionFactory;
		private readonly IFarmPhotoFileCopyProcessFactory _copyProcressFactory;

		public DefaultFarmPhotoService(
			IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory,
			IFarmPhotoFileCopyProcessFactory copyProcressFactory)
		{
			this._repositorySessionFactory = repositorySessionFactory
				?? throw new ArgumentNullException(nameof(repositorySessionFactory));

			this._copyProcressFactory = copyProcressFactory
				?? throw new ArgumentNullException(nameof(copyProcressFactory));
		}

		private async Task<(FarmPhotoEntityRepository Repository, FarmPhotoEntity Entity, FarmEntity Farm)>
			GetPhotoEntityAndEnsureFarmExistsAsync(int farmId, RepositorySession<IStatefulSessionAdapter> repositorySession)
		{
			
			var farm = await repositorySession.GetOrNotFoundExceptionAsync<FarmEntity, int, FarmEntityRepository>(farmId);

			var repository = repositorySession.Create<FarmPhotoEntityRepository>();
			var entity = await repository.Query().FirstOrDefaultAsync(fp => fp.Farm == farm);

			return (repository, entity, farm);
		}

		public async Task<bool> DeletePhotoAsync(int farmId)
		{
			using (var repositorySession = this._repositorySessionFactory.CreateSession())
			{
				var (repository, entity, _) = await this.GetPhotoEntityAndEnsureFarmExistsAsync(farmId, repositorySession);

				if (entity == null)
				{
					return false;
				}

				var existingPhoto = await UpdatePhotoAsync(null, repositorySession, repository, entity);
				return existingPhoto != null;
			}
		}

		public async Task<InMemoryFile> GetPhotoAsync(int farmId)
		{
			using (var repositorySession = this._repositorySessionFactory.CreateSession())
			{
				var (_, entity, _) = await this.GetPhotoEntityAndEnsureFarmExistsAsync(farmId, repositorySession);
				var existingPhoto = GetExistingPhoto(entity?.StorageRelativePath);

				if (existingPhoto == null)
				{
					return null;
				}

				var inMemoryFile = await InMemoryFile.CopyFromFileAsync(existingPhoto, entity.OriginalFileName);
				return inMemoryFile;
			}
		}

		private IDeletableFile GetExistingPhoto(string storageRelativePath)
		{
			if (String.IsNullOrEmpty(storageRelativePath))
			{
				return null;
			}

			var rootDirectory = this._copyProcressFactory.StorageRootDirectory;
			var fileInfo = new FileInfo(Path.Combine(rootDirectory, storageRelativePath));

			if (!fileInfo.Exists)
			{
				return null;
			}

			return new FileInfoDeletableFileAdapter(fileInfo);
		}

		public async Task SavePhotoAsync(int farmId, IFile photo)
		{
			if (photo == null)
			{
				throw new ArgumentNullException(nameof(photo));
			}

			using (var repositorySession = this._repositorySessionFactory.CreateSession())
			{
				var (repository, entity, farm) = await this.GetPhotoEntityAndEnsureFarmExistsAsync(farmId, repositorySession);

				_ = await UpdatePhotoAsync(photo, repositorySession, repository,
					entity ?? new FarmPhotoEntity() { Farm = farm });
			}
		}

		private async Task<IFile> UpdatePhotoAsync(IFile photo, RepositorySession<IStatefulSessionAdapter> repositorySession,
			FarmPhotoEntityRepository repository, FarmPhotoEntity entity)
		{
			var existingPhoto = this.GetExistingPhoto(entity.StorageRelativePath);
			var copyProcess = this._copyProcressFactory.Create(photo, existingPhoto);

			using (var transaction = repositorySession.Session.BeginTransaction())
			{
				await using (var fileCopyTransaction = new FileCopyTransaction(copyProcess))
				{
					var results = await fileCopyTransaction.CopyAllAsync(CancellationToken.None);

					if (photo == null)
					{
						await repository.DeleteAsync(entity);
					}
					else
					{
						var result = results[copyProcess];

						entity.OriginalFileName = photo?.Name; // NOTE: photo is null when deleting
						entity.StorageRelativePath = result.RelativePath;

						await repository.SaveOrUpdateAsync(entity);
					}

					await repositorySession.Session.FlushAsync();
					await fileCopyTransaction.CommitAsync();
					await transaction.CommitAsync();
				}
			}

			return existingPhoto;
		}
	}
}

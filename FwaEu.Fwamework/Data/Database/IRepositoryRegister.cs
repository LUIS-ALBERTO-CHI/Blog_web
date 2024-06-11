using FwaEu.Fwamework.Data.Database.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IRepositoryRegister
	{
		void Add<TEntity, TEntityImplementation, TRepository, TRepositoryImplementation>()
			where TEntity : class
			where TEntityImplementation : class
			where TRepository : class, IRepositoryBase
			where TRepositoryImplementation : class, TRepository, new();

		void Add<TRepository>()
			where TRepository : class, IRepository, new();

		Type GetByRepositoryType(Type repositoryType);
		Type GetByEntityType(Type entityType);
	}

	public class RepositoryRegister : IRepositoryRegister
	{
		private class RepositoryRegisterEntry
		{
			public Type EntityType { get; set; }
			public Type EntityImplementationType { get; set; }
			public Type RepositoryType { get; set; }
			public Type RepositoryImplementationType { get; set; }
		}

		private readonly List<RepositoryRegisterEntry> _entries = new List<RepositoryRegisterEntry>();

		public void Add<TRepository>()
			where TRepository : class, IRepository, new()
		{
			var entityDescriptor = new TRepository()
				.GetEntityDescriptor();

			this._entries.Add(new RepositoryRegisterEntry()
			{
				EntityType = entityDescriptor.EntityType,
				EntityImplementationType = entityDescriptor.EntityType,
				RepositoryType = typeof(TRepository),
				RepositoryImplementationType = typeof(TRepository),
			});
		}

		public void Add<TEntity, TEntityImplementation, TRepository, TRepositoryImplementation>()
			where TEntity : class
			where TEntityImplementation : class
			where TRepository : class, IRepositoryBase
			where TRepositoryImplementation : class, TRepository, new()
		{
			if (!typeof(IRepository).IsAssignableFrom(typeof(TRepositoryImplementation)))
			{
				throw new NotSupportedException(
					$"The provided type for {nameof(TRepositoryImplementation)} ({typeof(TRepositoryImplementation).FullName}) must implements {typeof(IRepository).FullName}.");
			}

			this._entries.Add(new RepositoryRegisterEntry()
			{
				EntityType = typeof(TEntity),
				EntityImplementationType = typeof(TEntityImplementation),
				RepositoryType = typeof(TRepository),
				RepositoryImplementationType = typeof(TRepositoryImplementation),
			});
		}

		public Type GetByRepositoryType(Type repositoryType)
		{
			return (this._entries.FirstOrDefault(entry => entry.RepositoryType == repositoryType)
				?? this._entries.First(entry => entry.RepositoryImplementationType == repositoryType))
				.RepositoryImplementationType;
		}

		public Type GetByEntityType(Type entityType)
		{
			return (this._entries.FirstOrDefault(entry => entry.EntityType == entityType) //NOTE: Search by entity interface or concrete type
				?? this._entries.First(entry => entry.EntityImplementationType == entityType)) // Or fallback on implementation type
			.RepositoryImplementationType;
		}
	}
}

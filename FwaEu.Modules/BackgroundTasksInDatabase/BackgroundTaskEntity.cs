using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasksInDatabase
{
	public class TaskStartParametersComponent : ITaskStartParameters
	{
		public static TaskStartParametersComponent From(ITaskStartParameters taskStartParameters)
		{
			return new TaskStartParametersComponent(
				taskStartParameters.TaskName,
				taskStartParameters.Argument);
		}

		//NOTE: For Nhibernate
		protected TaskStartParametersComponent()
		{
		}

		public TaskStartParametersComponent(string taskName, string argument)
		{
			this.TaskName = taskName ?? throw new ArgumentNullException(nameof(taskName));
			this.Argument = argument;
		}

		public string TaskName { get; protected set; }
		public string Argument { get; protected set; }
	}

	public class BackgroundTaskEntity : ICreationTracked
	{
		//NOTE: For Nhibernate
		protected BackgroundTaskEntity()
		{
		}

		public BackgroundTaskEntity(Guid queueGuid,
			TaskStartParametersComponent startParameters,
			DateTime queueDate)
		{
			this.QueueGuid = queueGuid;
			this.StartParameters = startParameters;
			this.QueueDate = queueDate;
		}

		public Guid QueueGuid { get; protected set; }
		public TaskStartParametersComponent StartParameters { get; protected set; }
		public DateTime QueueDate { get; protected set; }

		public DateTime? StartedOn { get; set; }
		public DateTime? EndedOn { get; set; }

		public bool WasCancelled { get; set; }
		public string ResultData { get; set; }
		public string ErrorMessage { get; set; }

		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }

		public bool IsNew()
		{
			//NOTE: No need for implementation currently (inherited from ICreationTracked -> IEntity)
			throw new NotSupportedException();
		}
	}

	public class BackgroundTaskEntityClassMap : ClassMap<BackgroundTaskEntity>
	{
		public BackgroundTaskEntityClassMap()
		{
			Not.LazyLoad();

			Id(entity => entity.QueueGuid).GeneratedBy.Assigned();

			Component(entity => entity.StartParameters, mapping =>
			{
				mapping.Map(sp => sp.TaskName).Not.Nullable();
				mapping.Map(sp => sp.Argument).Length(4001);
			});

			Map(entity => entity.QueueDate);
			Map(entity => entity.StartedOn);
			Map(entity => entity.EndedOn);

			Map(entity => entity.WasCancelled);
			Map(entity => entity.ResultData).Length(4001);
			Map(entity => entity.ErrorMessage).Length(2000);

			this.AddCreationTrackedPropertiesIntoMapping();
		}
	}

	public class BackgroundTaskEntityRepository : DefaultRepository<BackgroundTaskEntity, Guid>
	{
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.ProcessResults;

namespace FwaEu.Fwamework.Setup
{
	public class BeforeUpdateSchemaTask : CompositeTask<IBeforeUpdateSchemaSubTask>
	{
		private readonly IEnumerable<IBeforeUpdateSchemaSubTask> _subTasks;
		private readonly IImportProcessResultFactory _importProcessResultFactory;

		public BeforeUpdateSchemaTask(IEnumerable<IBeforeUpdateSchemaSubTask> subTasks,
			IImportProcessResultFactory importProcessResultFactory)
		{
			this._subTasks = subTasks
				?? throw new ArgumentNullException(nameof(subTasks));

			this._importProcessResultFactory = importProcessResultFactory
				?? throw new ArgumentNullException(nameof(importProcessResultFactory));
		}

		protected override ProcessResult CreateProcessResult()
		{
			return this._importProcessResultFactory.CreateProcessResult();
		}

		public override string Name => "BeforeUpdateSchema";

		protected override IEnumerable<ISubTask> GetSubTasks()
		{
			return this._subTasks;
		}
	}

	public interface IBeforeUpdateSchemaSubTask : ISubTask
	{
	}
}

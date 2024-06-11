using FwaEu.Modules.GenericImporter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporterCommon
{
	public class UserModelImporter : IModelImporter<UserModel>
	{
		public Task ImportAsync(DataReader reader, ModelImporterContext context)
		{
			return Task.CompletedTask;
		}
	}
}
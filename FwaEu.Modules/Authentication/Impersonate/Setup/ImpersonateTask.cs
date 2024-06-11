using FwaEu.Fwamework.ProcessResults;
using FwaEu.Fwamework.Setup;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Authentication.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.Impersonate.Setup
{
	public class ImpersonateTask : SetupTask<ImpersonateModel, SetupTaskResult<ImpersonateResultModel>>
	{
		public override string Name => "Impersonate";

		public ImpersonateTask(IImpersonateService impersonateService)
		{
			_impersonateService = impersonateService
				?? throw new ArgumentNullException(nameof(impersonateService));
		}

		private readonly IImpersonateService _impersonateService;

		public override async Task<SetupTaskResult<ImpersonateResultModel>> ExecuteAsync(ImpersonateModel arguments)
		{
			var processResult = new ProcessResult();

			var context = processResult.CreateContext(
				$"Impersonating with identity {arguments.Identity}", "ImpersonateSetupTask");

			var user = default(AuthenticatedUser);

			try
			{
				user = await _impersonateService.ImpersonateAsync(arguments.Identity);
			}
			catch (UserNotFoundException ex)
			{
				context.Add(ErrorProcessResultEntry.FromException("Invalid identity.", ex));
			}


			var result = default(ImpersonateResultModel);
			if (user != null)
			{
				context.Add(new InfoProcessResultEntry("Token created."));
				result = new ImpersonateResultModel(user.Token);
			}

			return new SetupTaskResult<ImpersonateResultModel>(processResult, result);
		}
	}

	public class ImpersonateModel
	{
		public string Identity { get; set; }
	}

	public class ImpersonateResultModel
	{
		public ImpersonateResultModel(string token)
		{
			Token = token ?? throw new ArgumentNullException(nameof(token));
		}

		public string Token { get; }
	}
}

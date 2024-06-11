using FwaEu.Fwamework.Setup.Authentication;
using FwaEu.Fwamework.Setup.Authentication.WebApi;
using FwaEu.Fwamework.Setup.FileImport;
using FwaEu.Fwamework.Setup.ImportableFiles;
using FwaEu.Modules.Data.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FwaEu.Fwamework.Setup
{
	public class SetupSecurityOptions : AuthenticationSchemeOptions
	{
		public const string SchemeName = "SetupScheme";
		public string Scheme => SchemeName;
		public const string Policy = "SetupPolicy";
	}

	public static class SetupExtensions
	{
		public static IServiceCollection AddFwameworkSetup(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			var section = context.Configuration.GetSection("Fwamework:Setup");
			services.Configure<SetupOptions>(section);

			services.AddTransient<ISetupTask, UpdateSchemaSetupTask>();
			services.AddTransient<ISetupTask, DropDatabaseSetupTask>();
			services.AddTransient<ISetupTask, DeleteTablesSetupTask>();
			services.AddTransient<ISetupTask, CreatePermissionsSetupTask>();
			services.AddTransient<ISetupTask, ListTablesSetupTask>();

			services.AddTransient<IFileListService, DefaultFileListService>();
			services.AddTransient<IFileIdService, Sha256HashFileIdService>();

			services.AddTransient<ISetupTask, ImportFileListTask>();
			services.AddTransient<ISetupTask, DownloadFilesTask>();
			services.AddTransient<ISetupTask, ImportFilesByIdTask>();

			services.AddTransient<ISetupTask, GetAllUsersTask>();

			services.AddTransient<ISetupTask, BeforeUpdateSchemaTask>();
			services.AddTransient<IBeforeUpdateSchemaSubTask, BeforeUpdateFilesImportSubTask>();

			services.AddTransient<ISetupTask, AfterUpdateSchemaTask>();
			services.AddTransient<IAfterUpdateSchemaSubTask, AfterUpdateFilesImportSubTask>();

			services.AddTransient<ISetupTask, Base64FileImportTask>();

			services.AddTransient<ISetupTask, StopApplicationSetupTask>();
			services.AddTransient<ISetupTask, ClearCachesSetupTask>();

			services.AddTransient<ISetupTask, ConnectionStringsSetupTask>();

			services.AddTransient<ISetupAuthenticationService, DefaultSetupAuthenticationService>();
			services.AddTransient<ISetupJsonWebTokenWriter, DefaultSetupJsonWebTokenWriter>();

			services.AddAuthorization(options =>
			{
				options.AddSetupPolicy();
			}).AddAuthentication(SetupSecurityOptions.SchemeName)
				.AddSetupAuthentication();

			return services;
		}

		public static AuthorizationOptions AddSetupPolicy(this AuthorizationOptions options)
		{
			var policy = new AuthorizationPolicyBuilder()
				.AddAuthenticationSchemes(SetupSecurityOptions.SchemeName)
				.AddAuthenticationSchemes(SetupTokenAuthenticationSchemeOptions.SchemeName)
				.RequireAuthenticatedUser()
				.Build();

			options.AddPolicy(SetupSecurityOptions.Policy, policy);
			return options;
		}

		public static AuthenticationBuilder AddSetupAuthentication(this AuthenticationBuilder authenticationBuilder)
		{
			authenticationBuilder.AddScheme<SetupSecurityOptions, SetupSecurityHandler>(SetupSecurityOptions.SchemeName, options => { });
			return authenticationBuilder.AddScheme<SetupTokenAuthenticationSchemeOptions, SetupTokenSecurityHandler>(
				SetupTokenAuthenticationSchemeOptions.SchemeName, options => { });
		}
	}
}

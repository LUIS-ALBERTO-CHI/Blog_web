using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FwaEu.Fwamework.StaticFiles;
using FwaEu.Fwamework.WebApi;
using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.BlogWeb.Initialization;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Swagger;
using FwaEu.Fwamework.Temporal;
using Microsoft.Extensions.Hosting;
using FwaEu.Fwamework.DependencyInjection;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.Permissions.ByRole;
using FwaEu.Fwamework;
using FwaEu.Fwamework.Setup;
using FwaEu.Fwamework.Imports;
using FwaEu.Modules.Importers.SqlImporter;
using FwaEu.Modules.Importers.ExcelImporter;
using FwaEu.Fwamework.Configuration;
using FwaEu.Modules.GenericImporter;
using FwaEu.Fwamework.ValueConverters;
using Serilog;
using FwaEu.Modules.Logging;
using FwaEu.Modules.Users.CultureSettings;
using FwaEu.Modules.MasterData;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.Users.AvatarsGravatar;
using FwaEu.Modules.Monitoring;
using FwaEu.Fwamework.Formatting;
using FwaEu.Modules.Importers.ExcelImporterGenericImporter;
using FwaEu.Modules.DataImport;
using FwaEu.Modules.Currencies;
using FwaEu.BlogWeb.FarmManager;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.PasswordRecovery;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FwaEu.Modules.Permissions.MasterData;
using FwaEu.Modules.ReportsMasterDataByEntities;
using FwaEu.Modules.ReportsProvidersByFileSystem;
using FwaEu.Modules.Reports;
using FwaEu.Modules.ReportsPerimeters;
using FwaEu.Modules.ReportsProvidersByEntities;
using FwaEu.Modules.BackgroundTasks;
using FwaEu.Modules.ReportsUserViewsByEntities;
using FwaEu.BlogWeb.Application;
using FwaEu.Modules.Users.AdminState;
using FwaEu.Modules.ReportsAdminProvidersByFileSystem;
using FwaEu.Modules.ReportsAdminProvidersByEntities;
using FwaEu.Modules.BackgroundTasksInDatabase;
using FwaEu.BlogWeb.FarmManager.Photo;
using FwaEu.Fwamework.Application;
using FwaEu.BlogWeb.Html;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.NHibernateLogging;
using FwaEu.Modules.Aspose;
using FwaEu.Fwamework.Text;
using FwaEu.Fwamework.Data;
using FwaEu.Modules.Permissions.ByUser;
using Microsoft.AspNetCore.Http;
using FwaEu.Modules.UserTasks;
using FwaEu.Modules.UserNotifications;
using FwaEu.Modules.SignalR;
using FwaEu.Modules.UserTasksUserNotifications;
using FwaEu.Modules.SearchEngine;
using FwaEu.BlogWeb.Users;
using FwaEu.Modules.ReportsSearchEngine;
using FwaEu.BlogWeb.ViewContext;
using FwaEu.Modules.EnumValues;
using FwaEu.Modules.AssemblyProvider;
using FwaEu.BlogWeb.AssemblyProvider;
using Microsoft.AspNetCore.Authorization;
using FwaEu.Modules.Authentication.JsonWebToken;
using FwaEu.Modules.Authentication.Impersonate;
using FwaEu.Fwamework.Mail;
using FwaEu.Modules.MasterDataUserNotifications;
using FwaEu.Modules.MasterDataImporter;

namespace FwaEu.BlogWeb
{
	public class Startup
	{
		public Startup(IWebHostEnvironment environment)
		{
			var builder = new ConfigurationBuilder()
				 .SetBasePath(environment.ContentRootPath)
				 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				 .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true);

			if (environment.EnvironmentName.Contains('.'))
			{
				//NOTE: This code allow us to have composed environments names
				//// ex: for "Production.VilliersLeBel" environment name, it will try to load appSettings.Production.json and appSettings.VilliersLebBel.json
				foreach (var environmentNamePart in environment.EnvironmentName.Split(".", StringSplitOptions.RemoveEmptyEntries))
				{
					builder = builder.AddJsonFile($"appsettings.{environmentNamePart}.json", optional: true, reloadOnChange: true);
					builder = builder.AddJsonFile($"appsettings.{environmentNamePart}.Local.json", optional: true, reloadOnChange: true);
				} 
			}

			builder = builder.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.Local.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables();

			this._configuration = builder.Build();
			this._environment = environment;

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(this._configuration)
				.CreateLogger();
		}

		private readonly IConfigurationRoot _configuration;
		private readonly IWebHostEnvironment _environment;

		public void ConfigureServices(IServiceCollection services)
		{
			using (var context = new ApplicationInitializationContext(this._configuration, this._environment))
			{
				context.ServiceStore.Add<IRepositoryRegister>(new RepositoryRegister());

				services.RemoveAll<IConfiguration>();
				services.RemoveAll<IConfigurationRoot>();
				services.AddSingleton<IConfiguration>(this._configuration);
				services.AddSingleton<IConfigurationRoot>(this._configuration);

				services.AddCors();
				services.AddMemoryCache();

				services.AddResponseCompression(options =>
				{
					options.EnableForHttps = true;
				});

				services.AddHttpContextAccessor();

				services.AddFwameworkDependencyInjection();
				services.AddFwameworkStaticFilesServices();
				services.AddFwameworkTemporal();
				services.AddFwameworkTextServices();
				services.AddFwameworkMvc(context);
				//allow the logging of Personal Identifiable Information
				//mandatory as we log these data. Else, we should deactivate to comply with RGPD
				//https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
				Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
				services.AddJsonWebTokenServices(context);
				var authenticationFeatures = services.AddFwameworkAuthentication(context);
				
				services.AddImpersonateServices(context, authenticationFeatures);
					
				services.AddAuthorization(options =>
				{
					options.DefaultPolicy = new AuthorizationPolicyBuilder()
					   .RequireAuthenticatedUser()
					   .AddAuthenticationSchemes(authenticationFeatures.AuthenticationSchemes.ToArray())
					   .Build();
				});
				

				services.AddFwameworkWebApiAllower(context);
				services.AddFwameworkSessionsServices();
				services.AddFwameworkNhibernate();
				services.AddFwameworkDatabase();
				services.AddFwameworkPermissions(context);
				services.AddFwameworkSetup(context);
				services.AddFwameworkImportServices();
				services.AddFwameworkValueConverters();
				services.AddFwameworkConfigurations();
				services.AddFwameworkUsersServices();
				services.AddFwameworkApplicationAllower(context);
				services.AddFwameworkGlobalization();
				services.AddFwameworkDataFileServices();
				services.AddFwameworkFormatting();
				services.AddFwameworkModelValidation();

				services.AddFwameworkModuleBackgroundTasksServices();
				services.AddFwameworkModuleBackgroundTasksInDatabaseServices(context);
				services.AddFwameworkModuleMasterDataImporter();
				services.AddFwameworkModuleDataImport();
				services.AddFwameworkModuleGenericImporter();
				services.AddFwameworkModuleSqlImportServices();
				services.AddFwameworkModuleExcelImportServices();
				services.AddFwameworkModuleExcelGenericImporter();
				services.AddFwameworkModuleCultureSettings(context);
				services.AddFwameworkModuleSerilog(context);
				services.AddFwameworkModuleAdminState(context);
				services.AddFwameworkModuleMasterDataServices();
				services.AddFwameworkModuleUserPerimeterServices();
				services.AddFwameworkModuleGravatar(context);
				services.AddFwameworkModuleMonitoring();
				services.AddFwameworkModuleCurrenciesServices(context);
				services.AddFwameworkModuleAllowApplicationEnabler(context);
				services.AddFwameworkModuleGenericAdmin();
				services.AddFwameworkModuleUserPasswordRecovery(context);
				services.AddFwameworkModulePermissionsMasterData();
				services.AddFwameworkModuleReports(builder => builder.AddMemoryCacheAsyncReportDataStoreService());
				services.AddFwameworkModuleReportsPerimeters(context);
				services.AddFwameworkModuleReportsMasterDataByEntities(context);

				if (context.Environment.IsDevelopment())
				{
					services.AddFwameworkModuleReportsProvidersByFileSystem(context);
				}

				services.AddFwameworkModuleReportsProvidersByEntities(context);
				services.AddFwameworkModuleReportsUserViewsByEntities(context);
				services.AddFwameworkModuleReportsAdmin();
				services.AddFwameworkModuleReportsAdminProvidersByFileSystem();
				services.AddFwameworkModuleReportsAdminProvidersByEntities();
				services.AddFwameworkModuleReportsSearchEngine();
				services.AddFwameworkModuleNHibernateLogging(context);
				services.AddFwameworkModuleUserTasks();
				services.AddFwameworkModuleSignalR(context);
				services.AddFwameworkModuleUserNotifications(context);
				services.AddFwameworkModuleUserTasksUserNotifications();
				services.AddFwameworkModuleSearchEngine();
				services.AddFwameworkModuleAspose(typeof(Aspose.Cells.License)); //NOTE: You can add other Aspose licenses depending on your application needs
				services.AddFwameworkModuleEnumValues();
				services.AddFwameworkModuleAssemblyProvider();

				services.AddFwameworkModulePermissionsByUser(context); //TODO: Choose between ByUser and others implementations
				services.AddFwameworkModulePermissionsByRole(context); //TODO: Choose between ByRole and others implementations

				services.AddApplicationInformation(context);
				services.AddRelativePathProviders(this._environment);
				services.AddApplicationAssemblyProvider();
				services.AddApplicationDatabase(context, authenticationFeatures);
				services.AddApplicationUserServices(context);
				services.AddApplicationMailServices(context);
				services.AddMailListenersServices();
				services.AddApplicationGlobalization();
				services.AddApplicationSwagger(authenticationFeatures, context);
				services.AddApplicationHtml(context);
				services.AddApplicationViewContext();

				services.AddApplicationFarmManager(context);
				services.AddApplicationFarmManagerPhoto(context);
				services.AddFwameworkModuleMasterDataUserNotifications();

				services.AddProblemDetails();
			}
		}

		public void Configure(IApplicationBuilder application, IApplicationInfo applicationInfo,
			ILoggerFactory loggerFactory, INHibernateLoggingInfo nHibernateLoggingInfo)
		{
			bool IsNotSetupRoute(HttpContext context)
			{
				return !context.Request.Path.StartsWithSegments("/Setup");
			}

			ApplicationServices.Initialize(application.ApplicationServices);

			application.ConfigureFwameworkSerilog(this._configuration, this._environment, applicationInfo);
			application.ConfigureNHibernateLogging(loggerFactory, nHibernateLoggingInfo);

			application.UseResponseCompression();

			if (this._environment.IsDevelopment())
			{
				application.UseDeveloperExceptionPage();
			}
			else if (this._configuration.GetSection("Application:UseHttpsRedirection").Get<bool>())
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				application.UseHsts();
				application.UseHttpsRedirection();
			}

			application.UseApplicationSwagger(this._configuration);
			application.UseFwameworkModuleLog();

			application.UseCors(options => options
				.SetIsOriginAllowed(_ => true) // NOTE: AllowAnyOrigin() not usable with AllowCredentials()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials());

			application.UseAuthentication();
			application.UseRouting();
			application.UseAuthorization();

			application.UseWhen(IsNotSetupRoute, application => application
				.UseFwameworkCurrentUser()
				.UseFwameworkCurrentUserPerimeter()
			);

			application.UseApplicationViewContext();

			application.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<UserNotificationHub>("/UserNotifications"); // TODO: Register routes from modules https://dev.azure.com/fwaeu/BlogWeb/_workitems/edit/6956
				endpoints.MapControllers();
			});
		}
	}
}

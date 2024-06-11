using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.Text;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Globalization
{
	public class LocalizableStringMappingConfig
	{
		/// <summary>
		/// If true, will create an unique key constraint for each culture individually.
		/// </summary>
		public bool UniqueValueByCulture { get; set; } = true;

		/// <summary>
		/// Allows you to set which columns will be nullable or not.
		/// </summary>
		public LocalizableStringNullable Nullable { get; set; }
			= LocalizableStringNullable.DefaultCultureNotNullable;

		/// <summary>
		/// Allows you to customize the Fluent PropertyPart.
		/// </summary>
		public OnColumnMapped OnColumnMapped { get; set; }
	}

	public enum LocalizableStringNullable
	{
		AllCulturesNullable,
		AllCulturesNotNullable,
		DefaultCultureNotNullable,
	}

	public delegate void OnColumnMapped(PropertyPart map, CultureInfo culture, bool isDefaultCulture);
	public delegate bool FilterCulture(CultureInfo culture, CultureInfo defaultCulture);

	public static class LocalizableStringClassMapExtensions
	{
		private static string NormalizeColumnName(string propertyName)
		{
			var databaseFeaturesType = ApplicationServices.ServiceProvider
				.GetRequiredService<ISessionFactoryProvider>()
				.DefaultConnectionInfo.DatabaseFeaturesType;

			var features = ApplicationServices.ServiceProvider
				.GetServices<IDatabaseFeaturesProvider>()
				.First(provider => provider.DatabaseFeaturesType == databaseFeaturesType)
				.GetDatabaseFeatures();

			var pluralizationService = ApplicationServices.ServiceProvider
				.GetRequiredService<IPluralizationService>();

			var conventions = new FwaConventions(
				new FwaConventions.FwaConventionsOptions(features),
				pluralizationService);

			// HACK: There is actually a bug here, because if the property name is long and the convention returns
			// the maximum length, appending _fr or _en will crash while creating the column in database. In this case,
			// we will need to manually set the column name.

			return conventions.GetPropertyColumnName(propertyName);

		}

		public static DynamicComponentPart<IDictionary> DynamicComponentLocalizableString<T>(
			this ClasslikeMapBase<T> mapper,
			Expression<Func<T, IDictionary>> memberExpression,
			LocalizableStringMappingConfig config = null,
			FilterCulture cultureFilter = null)
		{
			config ??= new LocalizableStringMappingConfig();
			cultureFilter ??= (culture, defaultCulture) => true;

			return mapper.DynamicComponent(memberExpression, dynamicComponent =>
			{
				var culturesService = ApplicationServices.ServiceProvider.GetRequiredService<ICulturesService>();
				var defaultCulture = culturesService.DefaultCulture;
				var propertyName = ((MemberExpression)memberExpression.Body).Member.Name;
				var columnNamePattern = NormalizeColumnName(propertyName) + "_{0}";

				foreach (var culture in culturesService.AvailableCultures
					.Where(c => cultureFilter(c, defaultCulture)))
				{
					var isDefaultCulture = culture == defaultCulture;

					var map = dynamicComponent.Map<string>(culture.TwoLetterISOLanguageName)
						.Column(String.Format(columnNamePattern, culture.TwoLetterISOLanguageName));

					if (config.UniqueValueByCulture)
					{
						map.Unique();
					}

					if (config.Nullable == LocalizableStringNullable.AllCulturesNotNullable
					|| (config.Nullable == LocalizableStringNullable.DefaultCultureNotNullable && isDefaultCulture))
					{
						map.Not.Nullable();
					}

					config.OnColumnMapped?.Invoke(map, culture, isDefaultCulture);
				}
			});
		}
	}
}
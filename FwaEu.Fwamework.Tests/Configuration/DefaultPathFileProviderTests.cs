using FwaEu.Fwamework.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FwaEu.Fwamework.Tests.Configuration
{
	[TestClass]
	public class DefaultPathFileProviderTests
	{
		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();
			services.AddTransient<IHostEnvironment, HostEnvironmentFake>();
			services.AddFwameworkConfigurations();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public void AllDefaultRelativePathsProviderAreProvided()
		{
			var constantsFields = typeof(DefaultRelativePaths)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Cast<FieldInfo>()
				.ToArray();

			using (var serviceProvider = BuildServiceProvider())
			{
				var providers = serviceProvider.GetServices<IRelativePathProvider>().ToArray();
				Assert.AreEqual(constantsFields.Length, providers.Length);

				foreach (var field in constantsFields)
				{
					var fieldValue = (string)field.GetRawConstantValue();
					var provider = providers.FirstOrDefault(p => p.RelativeTo == fieldValue);

					Assert.IsNotNull(provider, $"Provider not found with RelativeTo value = {fieldValue}.");
				}
			}
		}

		private const string FakeDirectoryRelativePath = "Configuration/FakeDirectory/";

		private static string[] GetFakeDirectoryExpectedFiles()
		{
			return new[]
			{
				"Sub1/a.txt",
				"Sub1/b.txt",
				"a.txt",
				"b.txt",
				"c.bat",
			};
		}

		[TestMethod]
		public void GetFiles_WildcardsAll()
		{
			var defaultPathFileProvider = new DefaultPathFileProvider(
				new DefaultRootPathProvider(Enumerable.Empty<IRelativePathProvider>()));

			var entry = new PathEntry()
			{
				Path = FakeDirectoryRelativePath + "*",
				RelativeTo = null,
			};

			var expectedFiles = GetFakeDirectoryExpectedFiles();
			var files = defaultPathFileProvider.GetFiles(new[] { entry })
				.Select(fibpe => fibpe.FileInfo)
				.ToArray();

			Assert.AreEqual(expectedFiles.Length, files.Length);

			var root = Path.GetFullPath(FakeDirectoryRelativePath);
			var foundFilesRelativePaths = files.Select(f => f.FullName.Substring(root.Length).Replace("\\", "/")).ToArray();
			var matches = foundFilesRelativePaths.Join(expectedFiles, f => f, ef => ef, (f, ef) => ef).ToArray();

			Assert.AreEqual(files.Length, matches.Length);
		}

		[TestMethod]
		public void GetFiles_WildcardsWithExtension()
		{
			var defaultPathFileProvider = new DefaultPathFileProvider(
					new DefaultRootPathProvider(Enumerable.Empty<IRelativePathProvider>()));

			var entry = new PathEntry()
			{
				Path = FakeDirectoryRelativePath + "*.txt",
				RelativeTo = null,
			};

			var fakeExpectedFiles = GetFakeDirectoryExpectedFiles();
			var testExpectedFiles = fakeExpectedFiles.Where(f => f.EndsWith(".txt")).ToArray();

			Assert.AreNotEqual(fakeExpectedFiles, testExpectedFiles);

			var files = defaultPathFileProvider.GetFiles(new[] { entry })
				.Select(fibpe => fibpe.FileInfo)
				.ToArray();

			Assert.AreEqual(testExpectedFiles.Length, files.Length);

			var root = Path.GetFullPath(FakeDirectoryRelativePath);
			var foundFilesRelativePaths = files.Select(f => f.FullName.Substring(root.Length).Replace("\\", "/")).ToArray();
			var matches = foundFilesRelativePaths.Join(testExpectedFiles, f => f, ef => ef, (f, ef) => ef).ToArray();

			Assert.AreEqual(files.Length, matches.Length);
		}
	}
}

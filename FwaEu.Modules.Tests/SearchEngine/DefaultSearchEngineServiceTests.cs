using FwaEu.Fwamework.Data;
using FwaEu.Modules.SearchEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.SearchEngine
{
	[TestClass]
	public class DefaultSearchEngineServiceTests
	{
		[TestMethod]
		public async Task SearchAsync_ShouldThrowProvidersNotFoundException()
		{
			var stubA = new SearchEngineResultProviderFactoryStub(
				() => new ExceptionSearchEngineResultProviderStub(), "A");

			var pagination = new SearchPagination(0, 5);

			var service = new DefaultSearchEngineService(new[] { stubA });

			await Assert.ThrowsExceptionAsync<ProvidersNotFoundException>(async () =>
				await service.SearchAsync("test", new[]
				{
					new SearchParameters("A", pagination),
					new SearchParameters("B", pagination),
					new SearchParameters("C", pagination),
				}));
		}

		[TestMethod]
		public async Task SearchAsync_ShouldThrowAggregateException()
		{
			var stubA = new SearchEngineResultProviderFactoryStub(
				() => new ExceptionSearchEngineResultProviderStub(), "A");

			var stubB = new SearchEngineResultProviderFactoryStub(
				() => new ExceptionSearchEngineResultProviderStub(), "B");

			var pagination = new SearchPagination(0, 5);

			var service = new DefaultSearchEngineService(new[] { stubA, stubB });

			await Assert.ThrowsExceptionAsync<AggregateException>(async () =>
				await service.SearchAsync("test", new[]
				{
					new SearchParameters("A", pagination),
					new SearchParameters("B", pagination),
				}));
		}
	}
}

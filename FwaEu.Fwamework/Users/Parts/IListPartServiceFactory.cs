using System;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Fwamework.Users.Parts
{
	public interface IListPartServiceFactory
	{
		IListPartService Create();
	}

	public class DefaultListPartServiceFactory : IListPartServiceFactory
	{
		public DefaultListPartServiceFactory(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider
				?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private readonly IServiceProvider _serviceProvider;

		public IListPartService Create()
		{
			return this._serviceProvider.GetService<IListPartService>();
		}
	}
}

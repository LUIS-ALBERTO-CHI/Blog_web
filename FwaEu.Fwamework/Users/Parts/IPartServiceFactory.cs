using System;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.Fwamework.Users.Parts
{
	public interface IPartServiceFactory
	{
		IPartService Create();
	}

	public class DefaultPartServiceFactory : IPartServiceFactory
	{
		public DefaultPartServiceFactory(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private readonly IServiceProvider _serviceProvider;

		public IPartService Create()
		{
			return this._serviceProvider.GetService<IPartService>();
		}
	}
}

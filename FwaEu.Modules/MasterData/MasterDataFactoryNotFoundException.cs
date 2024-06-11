using System;

namespace FwaEu.Modules.MasterData
{
	public class MasterDataFactoryNotFoundException : Exception
	{
		public MasterDataFactoryNotFoundException(string message) : base(message)
		{
		}
	}
}

using FwaEu.Fwamework.Temporal;
using System;
using System.Collections.Generic;
using System.Text;

namespace FwaEu.Modules.Tests.FileTransactions
{
	public class Fixed20171028CurrentDateTimeStub : ICurrentDateTime
	{
		public DateTime Now { get; } = new DateTime(2017, 10, 28, 4, 44, 28); //NOTE: Who will find what is that?
	}
}

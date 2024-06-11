using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Authentication
{
	public interface IClaimValueConvertService<TIn, TOut>
	{
		public TOut ToClaimValue(TIn value);
		public TIn FromClaimValue(TOut claimValue);
	}
}

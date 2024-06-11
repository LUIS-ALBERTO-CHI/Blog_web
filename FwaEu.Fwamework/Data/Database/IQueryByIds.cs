using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data.Database
{
	public interface IQueryByIds<TEntity, TIdentifier>
	{
		IQueryable<TEntity> QueryByIds(TIdentifier[] ids);
	}
}

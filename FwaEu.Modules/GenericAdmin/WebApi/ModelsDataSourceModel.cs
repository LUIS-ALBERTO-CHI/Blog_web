using System.Collections.Generic;
using System.Linq;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class DataSourceModel
	{
		protected DataSourceModel(List<object> data, int? totalCount)
		{
			this.Data = data;
			this.TotalCount = totalCount;
		}

		public List<object> Data { get; }
		public int? TotalCount { get; }
	}

	public interface IDataSourceModelFactory
	{
		/// <summary>
		/// Returns null if result type is not handled.
		/// </summary>
		DataSourceModel Create(IDataSource dataSource);
	}

	public class ListDataSourceModel : DataSourceModel
	{
		public ListDataSourceModel(List<object> items, int? totalCount) : base(items, totalCount)
		{
		}
	}

	public class ListDataSourceModelFactory : IDataSourceModelFactory
	{
		public DataSourceModel Create(IDataSource dataSource)
		{
			if (dataSource is ListDataSource listDataSource)
			{
				return new ListDataSourceModel(listDataSource.ListItems, listDataSource.TotalCount);
			}

			return null;
		}
	}

	public class EnumDataSourceModelFactory : IDataSourceModelFactory
	{
		public DataSourceModel Create(IDataSource dataSource)
		{
			if (dataSource is IEnumDataSource enumDataSource)
			{
				return new ListDataSourceModel(enumDataSource.Items.Cast<object>().ToList(), enumDataSource.TotalCount);
			}

			return null;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FwaEu.Modules.GenericAdmin
{
	public interface IDataSource
	{
		IEnumerable<object> Items { get; }
		int? TotalCount { get; }
	}

	public interface IDataSource<out TModel> : IDataSource
	{
		new IEnumerable<TModel> Items { get; }
	}

	public class LoadDataResult
	{
		public LoadDataResult(IDataSource value)
		{
			Value = value;
		}

		public IDataSource Value { get; private set; } //NOTE: Ready for later implementation of paginated load
	}

	public class LoadDataResult<TModel> : LoadDataResult
	{
		public LoadDataResult(IDataSource<TModel> value)
			: base(value)
		{
		}

		public new IDataSource<TModel> Value
		{
			get { return (IDataSource<TModel>)base.Value; }
		}
	}

	public class ListDataSource : IDataSource
	{
		public ListDataSource(List<object> items, int? totalCount)
		{
			ListItems = items;
			InnerTotalCount = totalCount;
		}

		public List<object> ListItems { get; }
		private int? InnerTotalCount { get; }

		public IEnumerable<object> Items => ListItems;
		public int? TotalCount => InnerTotalCount;
	}

	public class ListDataSource<TModel> : ListDataSource, IDataSource<TModel>
	{
		public ListDataSource(List<TModel> items, int? totalCount) : base(items.Cast<object>().ToList(), totalCount)
		{
			ListTypedItems = items;
		}

		public List<TModel> ListTypedItems { get; }

		public new IEnumerable<TModel> Items => ListTypedItems;
	}

	public class EnumValue
	{
		public EnumValue(object value)
		{
			Value = value;
		}

		public object Value { get; }
	}

	public interface IEnumDataSource : IDataSource
	{
		new IEnumerable<EnumValue> Items { get; }
	}

	public class EnumDataSource<TEnum> : IDataSource<EnumValue>, IEnumDataSource
		where TEnum : struct
	{
		static EnumDataSource()
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new NotSupportedException($"The '{nameof(TEnum)}' generic argument must be an enum.");
			}
		}

		public virtual IEnumerable<EnumValue> Items => Enum.GetValues(typeof(TEnum))
			.Cast<TEnum>().Select(value => new EnumValue(value));

		IEnumerable<object> IDataSource.Items => this.Items.Cast<object>();
		int? IDataSource.TotalCount => Enum.GetValues(typeof(TEnum)).Length;
	}
}
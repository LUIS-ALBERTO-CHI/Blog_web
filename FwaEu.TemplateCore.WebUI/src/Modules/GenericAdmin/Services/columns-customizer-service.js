const defaultIndex = 1000;
const customCustomizers = [];

class ColumnsCustomizer
{
	constructor()
	{
		this.columnsCustomizations = {};
		this.customColumnCustomizers = [];
	}

	addCustomization(key, options)
	{
		this.columnsCustomizations[key] = Object.assign(
			this.columnsCustomizations[key] ? this.columnsCustomizations[key] : {}, options);
	}

	async customizeAsync(columns, properties)
	{
		await Promise.all(customCustomizers.map(cc => cc.customizeAsync(columns, properties)));

		this.applyWidth(columns);
		return this.sortColumns(columns);
	}

	getColumnProperty(columnName, propertyName, defaultValue = undefined)
	{
		if (this.columnsCustomizations[columnName]
			&& typeof this.columnsCustomizations[columnName][propertyName] !== 'undefined')
		{
			return this.columnsCustomizations[columnName][propertyName];
		}
		return defaultValue;
	}

	getColumnWidth(columnName)
	{
		return this.getColumnProperty(columnName, 'width');
	}

	applyWidth(columns)
	{
		columns.forEach(c =>
		{
			const width = this.getColumnWidth(this.getColumnName(c));

			if (typeof width !== 'undefined')
			{
				c.width = width;
			}
		});
	}

	getColumnIndex(columnName)
	{
		return this.getColumnProperty(columnName, 'index', defaultIndex);
	}

	getColumnName(column) {
		return column.dataField ?? column.name
	}

	sortColumns(columns)
	{
		return columns.sort((a, b) =>
		{
			return this.getColumnIndex(this.getColumnName(a)) - this.getColumnIndex(this.getColumnName(b));
		});
	}
}

const columnCustomizer = new ColumnsCustomizer();

export default {
	getCustomizer()
	{
		return columnCustomizer;
	},
	registerCustomCustomizer(customizer) {
		customCustomizers.push(customizer);
	}
}
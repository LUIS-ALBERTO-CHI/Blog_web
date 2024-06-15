let dotNetTypesToDevExtremeConvertersRegistry = [];

export default {
	getAll()
	{
		return dotNetTypesToDevExtremeConvertersRegistry;
	},
	getConverter(dotNetType)
	{
		return dotNetTypesToDevExtremeConvertersRegistry
			.find(c => c.getDotNetTypesHandled().includes(dotNetType));
	},
	register(dotNetTypesToDevExtremeConverter)
	{
		dotNetTypesToDevExtremeConvertersRegistry.push(dotNetTypesToDevExtremeConverter);
	}
};
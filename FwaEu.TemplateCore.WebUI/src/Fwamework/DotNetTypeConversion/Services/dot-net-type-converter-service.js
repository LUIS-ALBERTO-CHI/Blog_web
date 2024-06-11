let dotNetTypesRegistry = [];

export default {
	getAll()
	{
		return dotNetTypesRegistry;
	},
	register(dotNetTypeConverter)
	{
		dotNetTypesRegistry.push(dotNetTypeConverter);
	}
};
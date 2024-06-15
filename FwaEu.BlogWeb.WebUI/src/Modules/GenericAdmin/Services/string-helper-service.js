export default {
	lowerFirstCharacter(string)
	{
		return string && string.length
			? string.substr(0, 1).toLowerCase() + string.substr(1)
			: string;
	},
	upperFirstCharacter(string)
	{
		return string && string.length
			? string.substr(0, 1).toUpperCase() + string.substr(1)
			: string;
	},
};
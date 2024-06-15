import AbstractDotNetTypeConverter from '@/Fwamework/DotNetTypeConversion/Services/abstract-dot-net-type-converter';

class GenericAdminLocalizableStringDictionaryConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['LocalizableString'];
	}

	getJavaScriptType()
	{
		return 'object';
	}

	getDisplayName()
	{
		return 'LocalizableString';
	}
}

export default new GenericAdminLocalizableStringDictionaryConverter();
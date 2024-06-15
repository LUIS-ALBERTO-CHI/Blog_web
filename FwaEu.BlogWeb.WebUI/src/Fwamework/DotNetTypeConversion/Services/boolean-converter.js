import AbstractDotNetTypeConverter from './abstract-dot-net-type-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class BooleanConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['Boolean'];
	}

	getJavaScriptType()
	{
		return 'boolean';
	}

	getDisplayName()
	{
		return I18n.t('Boolean');
	}
}

export default new BooleanConverter();
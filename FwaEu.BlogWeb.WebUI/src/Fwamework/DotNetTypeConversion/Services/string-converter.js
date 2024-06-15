import AbstractDotNetTypeConverter from './abstract-dot-net-type-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class StringConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['String'];
	}

	getJavaScriptType()
	{
		return 'string';
	}

	getDisplayName()
	{
		return I18n.t('String');
	}
}

export default new StringConverter();
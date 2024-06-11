import AbstractDotNetTypeConverter from './abstract-dot-net-type-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class DateConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['DateTime', 'DateTimeOffset'];
	}

	getJavaScriptType()
	{
		return 'date'; //NOTE: typeof new Date() returns object
	}

	getDisplayName()
	{
		return I18n.t('Date');
	}
}

export default new DateConverter();
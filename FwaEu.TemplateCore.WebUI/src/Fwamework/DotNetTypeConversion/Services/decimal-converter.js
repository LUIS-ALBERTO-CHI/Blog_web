import AbstractDotNetTypeConverter from './abstract-dot-net-type-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class DecimalConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['Decimal', 'Simple', 'Double'];
	}

	getJavaScriptType()
	{
		return 'number';
	}

	getDisplayName()
	{
		return I18n.t('Decimal');
	}
}

export default new DecimalConverter();
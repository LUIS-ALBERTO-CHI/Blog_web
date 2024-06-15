import AbstractDotNetTypeConverter from './abstract-dot-net-type-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class IntegerConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['UInt16', 'Int16', 'UInt32', 'Int32'];
	}

	getDefaultDotNetTypesHandled() {
		return 'Int32';
	}

	getJavaScriptType()
	{
		return 'number';
	}

	getDisplayName()
	{
		return I18n.t('Integer');
	}

	getDotNetTypesHandledForReporting() {
		return 'Int64';
	}
}

export default new IntegerConverter();
import AbstractDotNetTypeConverter from './abstract-dot-net-type-converter';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

class BigIntegerConverter extends AbstractDotNetTypeConverter
{
	constructor()
	{
		super();
	}

	getDotNetTypesHandled()
	{
		return ['UInt64', 'Int64'];
	}

	getJavaScriptType()
	{
		return 'bigint';
	}

	getDisplayName()
	{
		return I18n.t('BigInteger');
	}
}

export default new BigIntegerConverter();
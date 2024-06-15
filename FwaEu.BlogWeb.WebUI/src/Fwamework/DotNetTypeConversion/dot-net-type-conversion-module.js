import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import DotNetTypeConverterService from "@/Fwamework/DotNetTypeConversion/Services/dot-net-type-converter-service";
import BooleanConverter from "@/Fwamework/DotNetTypeConversion/Services/boolean-converter";
import DateTimeConverter from "@/Fwamework/DotNetTypeConversion/Services/date-converter";
import DecimalConverter from "@/Fwamework/DotNetTypeConversion/Services/decimal-converter";
import IntegerConverter from "@/Fwamework/DotNetTypeConversion/Services/integer-converter";
import BigIntegerConverter from "@/Fwamework/DotNetTypeConversion/Services/big-integer-converter";
import StringConverter from "@/Fwamework/DotNetTypeConversion/Services/string-converter";

export class DotNetTypeConversionModule extends AbstractModule
{
	async onInitAsync()
	{
		DotNetTypeConverterService.register(BooleanConverter);
		DotNetTypeConverterService.register(DateTimeConverter);
		DotNetTypeConverterService.register(DecimalConverter);
		DotNetTypeConverterService.register(IntegerConverter);
		DotNetTypeConverterService.register(BigIntegerConverter);
		DotNetTypeConverterService.register(StringConverter);
	}
}
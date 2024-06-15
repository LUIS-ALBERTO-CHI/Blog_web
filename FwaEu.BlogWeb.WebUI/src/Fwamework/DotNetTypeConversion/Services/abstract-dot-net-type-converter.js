class AbstractDotNetTypeConverter
{
	constructor()
	{
		if (this.constructor === AbstractDotNetTypeConverter)
		{
			throw new TypeError('Abstract class "AbstractDotNetTypeConverter" cannot be instantiated directly');
		}
	}

	getDotNetTypesHandled()
	{
		throw new Error('You must implement the function getDotNetTypesHandled!');
	}

	getJavaScriptType()
	{
		throw new Error('You must implement the function getJavaScriptType!');
	}

	getDisplayName()
	{
		throw new Error('You must implement the function getDisplayName!');
	} 

	getDefaultDotNetTypesHandled() {
		return this.getDotNetTypesHandled()[0];
	}

	getDotNetTypesHandledForReporting() {
		return this.getDefaultDotNetTypesHandled();
	}
}

export default AbstractDotNetTypeConverter;
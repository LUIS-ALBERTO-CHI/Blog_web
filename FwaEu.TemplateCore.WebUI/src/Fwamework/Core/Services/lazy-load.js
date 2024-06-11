function Lazy(valueFactory) {
	if (typeof valueFactory !== 'function') {
		throw 'valueFactory must be a reference to a function.';
	}

	let value = null;

	this.getValue = function Lazy$getValue() {
		if (valueFactory) {
			value = valueFactory();
			valueFactory = null;
		}

		return value;
	}
}

function AsyncLazy(asyncValueFactory) {
	if (typeof asyncValueFactory !== 'function') {
		throw 'asyncValueFactory must be a reference to a function.';
	}

	let value = null;
	let runningValueFactory = null;

	this.getValueAsync = async function Lazy$getValueAsync() {
		if (asyncValueFactory) {
			//NOTE: Clear the factory value before calling it because of microtasks queue priority over tasks queue
			const asyncValueFactoryCopy = asyncValueFactory;
			asyncValueFactory = null;

			//NOTE: Save the current loading call in order to reuse it if someone tries to retrieve the value during the value initialization
			runningValueFactory = asyncValueFactoryCopy(),
			value = await runningValueFactory;
			runningValueFactory = null;
		} else if (runningValueFactory) {
			return await runningValueFactory;
		}

		return value;
	}
}

export { Lazy, AsyncLazy };
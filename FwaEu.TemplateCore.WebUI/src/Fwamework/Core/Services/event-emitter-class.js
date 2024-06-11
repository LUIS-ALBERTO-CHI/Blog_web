

class AsyncEventEmitter {

	constructor(parallelInvoke = false) {
		this.parallelInvoke = parallelInvoke;
		this.listeners = [];
	}
	/** Adds a listener and returns the necessary function to remove the listener
	 *  @param {()=> Promise} eventListener @returns {()=> void } */
	addListener(eventListener) {
		if (typeof eventListener !== "function") {
			throw new Error("Event listener must be a function");
		}
		this.listeners.push(eventListener);
		const $this = this;
		return () => {
			$this.listeners.splice($this.listeners.indexOf(eventListener), 1);
		};
	}

	/**
	 * @param {any} e
	 * @param {(any) => boolean} stopCondition
	 */
	async emitAsync(e, stopCondition = null) {
		if (this.parallelInvoke) {
			await Promise.all(this.listeners.map(listener => listener(e)));
		} else {
			for (let listener of this.listeners) {
				let invokeResult = await listener(e);
				if (stopCondition && stopCondition(invokeResult)) {
					break;
				}
			}
		}
	}
}
export default AsyncEventEmitter;
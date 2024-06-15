/** Tries to run the requested async function and return the result when the execution will be completed */
export default class AsyncQueue {

	constructor(maxParallel = 1, reuseExisting = false) {

		this._queue = new Set();
		this._maxParallel = maxParallel;
		this._reuseExisting = reuseExisting;
	}

	/** @param {() => Promise } asyncFunction */
	async runAsync(asyncFunction) {

		//If there is not available place for a new execution and reuse is not enabled
		//then we wait for the first running function to be finished before retrying the current asyncFunction execution
		if (this._queue.size >= this._maxParallel && !this._reuseExisting) {

			await this._queue.values().next().value;
			return await this.runAsync(asyncFunction);
		}

		let currentPromise;
		let result;
		if (this._queue.size < this._maxParallel) {

			currentPromise = asyncFunction();
			this._queue.add(currentPromise);
		} else if (this._reuseExisting) {

			currentPromise = this._queue.values().next().value;
		} else {
			return await this.runAsync(asyncFunction);
		}

		try {
			result = await currentPromise;
		} catch (error) {
			console.error(error);
		} finally {
			this._queue.delete(currentPromise);
		}

		return result;
	}
}
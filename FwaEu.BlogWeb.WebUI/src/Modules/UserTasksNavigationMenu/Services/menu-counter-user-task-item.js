export class MenuCounterUserTaskItem {

	constructor(userTaskName) {
		this.name = userTaskName;
		this.count = 0;
		this.minDisplay = 1;
		this.maxCount = 99;
		this.params = {};
	}

	/**@param {Number | {count: Number}} value */
	setValue(value) {
		let count = value?.count === undefined
			? value
			: value.count;
		if (!count)
			return;

		this.count = count;
		this.params = value?.constructor.name === 'Object' ? { ...value } : { count: value };
	}
}
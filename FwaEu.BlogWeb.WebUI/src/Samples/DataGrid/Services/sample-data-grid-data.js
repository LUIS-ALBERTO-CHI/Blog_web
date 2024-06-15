
function buildData() {
	const result = [];
	for (let i = 1; i <= 20; ++i) {
		result.push({
			id: i,
			name: `Line ${i}`,
			postalCodeId: i
		});
	}
	return result;
}

const dataSource = buildData();

export default {
	getSampleDataAsync: (fakeAsyncDelay) => {
		return new Promise((resolve, reject) => setTimeout(() => resolve(dataSource), fakeAsyncDelay));
	},
	getPaginatedSampleDataAsync: (fakeAsyncDelay, options) => {
		return new Promise((resolve, reject) => setTimeout(() => {
			resolve({
				data: dataSource.slice(options.skip, options.skip + options.take),
				totalCount: dataSource.length
			});
		}, fakeAsyncDelay));
	}
};
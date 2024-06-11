const processResultCustomizers = import.meta.glob('/**/*-process-result-customizer.js');

export default
	{
		async customizeAsync(processResult) {
			for (let i = 0; i < processResult.contexts.length; i++) {
				var context = processResult.contexts[i];

				for (const value in processResultCustomizers) {

					let processResultValue = await processResultCustomizers[value]();
					await processResultValue.default.customizeAsync(context);

				}
				processResult.contexts[i] = context;
			}
		}
	};
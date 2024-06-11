import HttpService from '@/Fwamework/Core/Services/http-service';
import ProcessResultService from '@/Fwamework/ProcessResults/Services/process-result-service';

export default
	{
		async importDataAsync(files)
		{
			let formData = new FormData();

			for (var i = 0; i < files.length; i++)
			{
				let file = files[i];
				formData.append('files[' + i + ']', file);
			}

			var response = await HttpService.post("DataImport/Import",
				formData,
				{
					headers: {
						'Content-Type': 'multipart/form-data'
					}
				}
			);

			var data = response.data;
			await ProcessResultService.customizeAsync(data.results);
			return data;
		}
	}
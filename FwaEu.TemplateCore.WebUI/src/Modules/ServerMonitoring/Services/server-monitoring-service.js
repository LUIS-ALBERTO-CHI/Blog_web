import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async getApplicationInfoAsync() {
		let response = await HttpService.get('Monitoring/ApplicationInfo');
		return response.data;
	},

	async pingAsync() {
		let response = await HttpService.get('Monitoring/Ping', { headers: {  "ApplicationSecret": "d2VidWljb2Rlc2VjcmV0=" } });
		return response.data;
	}
}
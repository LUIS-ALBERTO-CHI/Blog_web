import ServerMonitoringService from "./server-monitoring-service";

export default {
	async getAsync() {
		return await ServerMonitoringService.getApplicationInfoAsync();
	}
}
import HttpService from "@/Fwamework/Core/Services/http-service";

export default {
    async getAllAsync() {
        const notifications = await HttpService.get('Notifications');
        return notifications.data;
    },
    async markAsSeenAsync(date) {
        return await HttpService.post('Notifications/Seen',new Date(date));
    },
    async deleteAsync(id) {
        return await HttpService.delete(`Notifications/${id}`);
    }
}
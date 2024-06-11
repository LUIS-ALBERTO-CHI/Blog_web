import localizationService from "@/Fwamework/Culture/Services/localization-service";

const currentLanguage = localizationService.getCurrentLanguage()
export default {
	getMonthNames(months = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11], format = 'long') {
		return months.map(m => {
			const date = new Date(Date.UTC(2021, m));
			return new Intl.DateTimeFormat(currentLanguage, { month: format }).format(date);
		})
	},
	getDaysOfWeekNames(daysOfWeek = [0, 1, 2, 3, 4, 5, 6], format = 'short') {
		return daysOfWeek.map(dayOfWeek => {
			const date = new Date();
			date.setDate(date.getDate() + (dayOfWeek - date.getDay()));
			return new Intl.DateTimeFormat(currentLanguage, { weekday: format }).format(date);
		})
	}
}
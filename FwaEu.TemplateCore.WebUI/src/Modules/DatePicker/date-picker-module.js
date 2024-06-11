import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import Datepicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'

export class DatePickerModule extends AbstractModule {
	onInitAsync(vueApp) {
		vueApp.component('date-picker', Datepicker);
	}
}
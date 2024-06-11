import { createDateLiteral } from '@/Fwamework/Utils/Services/date-literal-helper';

export default {
	type: "date",
	async createCellCustomTemplateAsync(container, data, property) {
		if (data.value) {
			const props = {
				date: new Date(data.value)
			};
			if (property.format)
				props.displayFormat = property.format;

			const componentInstance = createDateLiteral(props);
			data.component.on('disposing', function () {
				componentInstance.unmount();
			});

			container.appendChild(componentInstance.$el);			
		}
	}
}

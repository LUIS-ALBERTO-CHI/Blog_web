import { createRouterLink } from '@/Fwamework/Routing/Services/router-link-helper';
const urlParameters = /{([^}]+)}/g;

export default {
	type: "url",
	async createCellCustomTemplateAsync(container, data, property) {
		const path = property.urlFormat.replace(urlParameters, function (_, token) { return data.data[token]; });
		const componentInstance = createRouterLink({ to: path }, data.data[property.fieldName]);
		data.component.on('disposing', function () {
			componentInstance.unmount();
		});
		container.appendChild(componentInstance.$el);

	}
}

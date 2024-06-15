import { createRouterLink } from '@/Fwamework/Routing/Services/router-link-helper';
import { createUserAvatar } from './user-avatar-helper';

export default {
	createColumns(component) {

		let listColumnsCreated = [
			{
				width: 80,
				type: "buttons",
				allowFiltering: false,
				cellTemplate(cellElement, cellInfo) {
					const id = cellInfo.data.id;
					const componentInstance = createRouterLink({ to: { name: 'EditUserDetails', params: { id } } }, component.$i18n.t("edit"));
					cellInfo.component.on('disposing', function () {
						componentInstance.unmount();
					});
					cellElement.appendChild(componentInstance.$el);
				}
			},
			{
				dataField: "initials",
				caption: "",
				width: 50,
				alignment: 'center',
				allowFiltering: false,
				cellTemplate(cellElement, cellInfo) {
					const componentInstance = createUserAvatar({ user: cellInfo.data });
					cellInfo.component.on('disposing', function () {
						componentInstance.unmount();
					});
					cellElement.appendChild(componentInstance.$el);
				}
			},
			{
				dataField: 'id',
				allowFiltering: false,
				width: 60
			}
		];

		return listColumnsCreated;
	}
};
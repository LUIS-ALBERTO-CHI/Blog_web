import { confirm, alert, custom } from 'devextreme/ui/dialog';

export default {
	initialize() {
		//NOTE: Not required for DevExtreme implementation
	},
	configure() {
		//NOTE: Not required for DevExtreme implementation
	},
	async confirmAsync(message, title = null) {
		return await confirm(message, title);
	},
	async alertAsync(message, title = null) {
		return await alert(message, title);
	},
	/// <summary>
	/// buttons is an array of objects that can contain the following properties:
	///   - text : The button text
	///   - type : The button type, possible values are 'back' | 'danger' | 'default' | 'normal' | 'success' (see DevExtreme documentation for available types)
	///   - style: The button style, possible values are 'text' | 'outlined' | 'contained'  (see DevExtreme documentation for available types)
	///   - value: The button value if clicked, by default, values are the button index
	/// </summary>
	customAsync(message, buttons, title = null) {
		return new Promise(function (resolver, reject) {
			let buttonIndex = 0;
			let dialogBox = custom({
				title: title,
				messageHtml: message,
				buttons: buttons.map(function (item) {
					const currentButtonIndex = buttonIndex++;
					return {
						text: item.text,
						type: item.type,
						style: item.style,
						onClick: function () { resolver(item.value ?? currentButtonIndex); }
					};
				})
			});
			dialogBox.show();
		});
	}
};
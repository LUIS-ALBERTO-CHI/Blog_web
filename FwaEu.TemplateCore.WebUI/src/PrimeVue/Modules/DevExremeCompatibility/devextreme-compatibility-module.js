import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import { addComponentMapping } from '../../module';

export class DevExtremeCompatibilityModule extends AbstractModule {
	onInitAsync() {
		addComponentMapping("dxTextBox", "InputText");
		addComponentMapping("dxSelectBox", "Dropdown");
		addComponentMapping("dxNumberBox", "InputNumber");
		addComponentMapping("dxCheckBox", "Checkbox");
		addComponentMapping("dxDateBox", "Calendar");
		addComponentMapping("dxSwitch", "InputSwitch");
	}
}
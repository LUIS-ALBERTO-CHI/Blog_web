import DataGrid from 'devextreme/ui/data_grid';
import DateBox from "devextreme/ui/date_box";
import TreeList from "devextreme/ui/tree_list";
import Tooltip from "devextreme/ui/tooltip";
import SelectBox from "devextreme/ui/select_box";
import Form from "devextreme/ui/form";
import TranslationOverrides from '@UILibrary/Extensions/Content/translation-overrides';
import { loadMessages } from 'devextreme/localization';
import LoadingPanelService from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
import NumberBox from 'devextreme/ui/number_box';
import NumberLocalization from 'devextreme/localization/number';
import ErrorHandlerService from '@/Fwamework/Errors/Services/error-handler-service';

export default {
	applyDefaultRules() {

		NumberBox.defaultOptions({
			options: {
				inputAttr: {
					style: "text-align: right"
				},
				onKeyDown(e) {

					//HACK: Handle Decimal separator on AZERTY keyboard, change the "." from NumPad to ","
					const currentDecimalSeparator = NumberLocalization.getDecimalSeparator();
					if (currentDecimalSeparator !== e.event.key && e.event.keyCode === 110) {
						e.event.preventDefault();
						e.event.stopPropagation();
						const input = e.element.querySelector("input.dx-texteditor-input");
						if (!input.value.includes(currentDecimalSeparator)) {
							e.component._setInputText(input.value + currentDecimalSeparator);
							input.setSelectionRange(input.value.length, input.value.length);
						}
					}
				}
			}
		});

		DataGrid.defaultOptions({
			options: {
				wordWrapEnabled: true,
				showBorders: false,
				rowAlternationEnabled: true,
				showColumnLines: false,
				errorRowEnabled: false,
				onDataErrorOccurred(e) {
					ErrorHandlerService.dispatchError(e.error);
				},
				editing: {
					useIcons: true,
				},
				toolbar: {
					items: [{
						name: 'addRowButton',
						location: 'before'
					},
					{
						name: 'exportButton'
					},
					{
						name: 'applyFilterButton'
					},
					{
						name: 'columnChooserButton'
					},
					{
						name: 'revertButton'
					},
					{
						name: 'saveButton'
					},
					{
						name: 'groupPanel'
					},
					{
						name: 'searchPanel'
					}]
				},
				onInitialized(e) {

					//HACK: hide save grid instance into loadingPanel instance
					const gridView = e.component;
					const rowsView = gridView._views.rowsView;
					rowsView._fwaInnerLoadPanel = null;

					Object.defineProperty(rowsView, '_loadPanel', {
						get: function () {
							return rowsView._fwaInnerLoadPanel;
						},
						set: function (value) {
							rowsView._fwaInnerLoadPanel = value;
							if (rowsView._fwaInnerLoadPanel)
								rowsView._fwaInnerLoadPanel._gridView = gridView;
						},

					});
				},
				//Globally replace the DataGrid loading panel to use the application modal loading panel
				loadPanel: {
					onShowing(e) {
						//HACK: hide native grid indicator and loaders only for first load
						if (e.component._gridView?.getDataSource()._changedTime === 0) {
							e.component._showFwaModal = true;
							e.component._$indicator.hide();
							e.component._$content.hide();
							LoadingPanelService.showModal();

						} else {
							e.component._$indicator.show();
							e.component._$content.show();
						}
					},
					onHiding(e) {
						if (e.component._showFwaModal) {
							e.component._showFwaModal = false;
							LoadingPanelService.hideModal();
							e.component._gridView?._options._optionManager.set({
								loadPanel: { delay: 500 }
							});
						}
					}
				},
				customizeColumns: function (columns) {
					if (columns) {
						for (let col of columns) {
							if (!col.sortingMethod) {
								col.sortingMethod = (x, y) => {
									if (typeof x === 'string' && typeof y === 'string')
										return Intl.Collator().compare(x ?? '', y ?? '');
									if ((x === null || x === undefined)
										&& (y !== null && y !== undefined)) {
										return -1;
									}
									if ((x !== null && x !== undefined)
										&& (y === null || y === undefined)) {
										return 1;
									}
									if (x < y) {
										return -1;
									}
									if (x > y) {
										return 1;
									}
									return 0;
								}
							}
						}
					}
				}
			}
		});
		DateBox.defaultOptions({
			options: {
				dateSerializationFormat: "yyyy-MM-ddTHH:mm:ss",
				showClearButton: true
			}
		});
		TreeList.defaultOptions({
			options: {
				showRowLines: true,
				showBorders: true,
				columnAutoWidth: true,
				onDataErrorOccurred(e) {
					throw e.error;
				}
			}
		});
		Form.defaultOptions({
			options: {
				showColonAfterLabel: false,
				showRequiredMark: true,
				labelLocation: "top"
			}
		});
		Tooltip.defaultOptions({
			options: {
				showEvent: { name: 'mouseover', delay: 150 },
				hideEvent: { name: 'mouseout', delay: 150 }
			}
		});
		SelectBox.defaultOptions({
			options: {
				displayExpr: (item) => item?.toString(),
				searchEnabled: true,
				showClearButton: true
			}
		});
	},
	applyTranslationOverrides() {
		loadMessages(TranslationOverrides);
	}
};

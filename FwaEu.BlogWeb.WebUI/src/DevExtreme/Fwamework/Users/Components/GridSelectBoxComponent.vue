<template>
    <div class="fwa-grid-select-box-container">
        <dx-select-box :data-source="dataSourceGlobal"
                       field-template="field"
                       item-template="item"
                       :show-clear-button="false"
                       :open-on-field-click="true"
                       :field-edit-enabled="false"
                       :search-enabled="false"
                       @content-ready="onContentReady"
                       @selection-changed="onSelectBoxChange"
                       @item-click="onSelectBoxClick">

            <template #field>
                <div>
                    <dx-text-box :value="selectedValueToDisplay"
                                 :show-clear-button="true"
                                 @input="onInput" />
                </div>
            </template>
            <template #item>
                <div>
                    <dx-data-grid v-bind="gridOptions"
                                  :dataSource="dataSource"
                                  @initialized="initialized"
                                  :columns="columns"
                                  :height="250"
                                  :selection="{ mode: 'single' }"
                                  :scrolling="{ mode: 'virtual'}"
                                  @selection-changed="onDataGridChange" />
                </div>
            </template>
        </dx-select-box>
    </div>
</template>

<script>
    import DxSelectBox from 'devextreme-vue/select-box';
    import DxDataGrid from 'devextreme-vue/data-grid';
    import { DxTextBox } from 'devextreme-vue/text-box';
    import { format } from '@intlify/shared';

    export default {
        components: {
            DxSelectBox,
            DxDataGrid,
            DxTextBox
        },
        props: {
            gridOptions: {
                type: Object,
                default: () => { }
            },
            dataSource: {
                type: [Array, Object]
            },
            columns: {
                type: Array
            },
            selectedItem: {
                type: Object,
                default: () => { }
            },
            displayFormat: {
                type: String,
            },
            defaultSeparator: {
                type: String,
                default: ', '
            }
        },
        data() {
            return {
                grid: null,
                opened: true,
                popup: null,
                searchTimer: null
            };
        },
        methods: {
            onSelectBoxChange(e) {
                if (e.selectedItem == null) {
                    if (this.grid != null) { this.grid.clearSelection(); }
                    e.component.option("opened", false);
                }
            },
            onDataGridChange(e) {
                this.grid = e.component;
                if (e.selectedRowsData.length != 0) {
                    this.opened = false;
                    if (this.popup) {
                        this.popup._hide();
                    }
                    var selected = e.selectedRowsData[0];
                    this.$emit('update:selectedItem', selected);
                }
            },
            onSelectBoxClick(e) {
                e.component.option("opened", this.opened);
                this.opened = true;
            },
            initialized(e) {
                this.grid = e.component;
            },
            onContentReady(e) {
                if (e.component._popup) {
                    e.component._popup.option('closeOnTargetScroll', false);
                    this.popup = e.component._popup;
                    this.popup._$content.addClass("fwa-grid-select-box");
                }
            },
            onInput(e) {
                clearTimeout(this.searchTimer);
                this.searchTimer = setTimeout(() => {
                    this.grid.searchByText(e.component.option("text"));
                }, 300);
            },
            getDataField(column) {
                if (!column) return null;
                return typeof column === 'string' || column instanceof String ? column : column.dataField;
            },
            getColumnValue(dataField) {
                return dataField.split('.').reduce((agg, pn) => {
                    if (agg && typeof agg === 'object') {
                        return agg[pn];
                    }
                    return agg;
                }, this.selectedItem);
            }
        },
        computed: {
            dataSourceGlobal() {
                return [{ dataSource: this.dataSource }];
            },
            selectedValueToDisplay() {
                if (this.selectedItem) {
                    var values = this.columns.map(col => this.getColumnValue(this.getDataField(col)));

                    if (!this.displayFormat)
                        return values.filter(x => x !== null && x !== undefined).join(this.defaultSeparator);

					return format(this.displayFormat, values);
                }
                return null;
            }
        }
    }
</script>
<style>
    .fwa-grid-select-box .dx-list-item-content {
        padding: 0px !important;
    }

    .fwa-grid-select-box .dx-dropdownlist-popup-wrapper .dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item-content {
        padding: 0px !important;
    }

    .fwa-grid-select-box-container .dx-template-wrapper {
        width: 100%;
    }
</style>
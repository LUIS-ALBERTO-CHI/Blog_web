<template>
	<div class="menu-list">
		<dx-tree-list :data-source="menuItems"
					  :auto-expand-all="true"
					  :show-column-headers="false"
					  :show-column-lines="false"
					  :show-borders="false"
					  :showColumnHeaders="false"
					  :scrolling="{mode: 'standard'}"
					  key-expr="text"
					  items-expr="items"
					  data-structure="tree"
					  @selection-changed="onSelectionChanged">
			<dx-search-panel :visible="true" :width="250" />
			<dx-selection mode="single"></dx-selection>
			<dx-column data-field="text" :width="250"
					   cell-template="routerLinkCellTemplate"
					   :calculate-cell-value="getNormalizedTextValue"
					   :calculate-filter-expression="textCalculatedFilter">
			</dx-column>
			<dx-column data-field="descriptionText">
			</dx-column>
			<dx-column :visible="false"
					   :calculate-cell-value="getNormalizedDescription"
					   :calculate-filter-expression="descriptionCalculatedFilter">
			</dx-column>
			<template #routerLinkCellTemplate="{ data }">
				<span v-if="data.data.path">
					<i :class="data.data.icon + ' menu-list-item-icon'" />
					<router-link :to="data.data.path">{{data.data.text}}</router-link>
				</span>
				<span v-else>
					{{data.data.text}}
				</span>
			</template>
		</dx-tree-list>
	</div>
</template>
<script>
	import { DxTreeList, DxSearchPanel, DxColumn, DxSelection } from 'devextreme-vue/tree-list';

	export default {
		components: {
			DxTreeList,
			DxSearchPanel,
			DxColumn,
			DxSelection,
		},
		data(vm) {
			return {
				textCalculatedFilter(filterValue, selectedFilterOperation, target) {
					const column = this;
					return vm.getCalculatedFilter(filterValue, selectedFilterOperation, target, column, (data) => {
						return data.text?.getNormalizedText();
					});
				},
				descriptionCalculatedFilter(filterValue, selectedFilterOperation, target) {
					const column = this;
					return vm.getCalculatedFilter(filterValue, selectedFilterOperation, target, column, (data) => {
						return data.descriptionText?.getNormalizedText();
					});
				}
			}
		},
		props: {
			menuItems: {
				type: Array,
				required: true
			}
		},
		methods: {
			onSelectionChanged(e) {
				e.component.deselectAll();
			},
			getCalculatedFilter(filterValue, selectedFilterOperation, target, column, getter) {
				if (target == "search") {
					filterValue = filterValue?.getNormalizedText();
					return [getter, selectedFilterOperation || "contains", filterValue];
				}
				return column.defaultCalculateFilterExpression(filterValue, selectedFilterOperation, target);
			},
			getNormalizedTextValue(rowData) {
				return rowData.text?.getNormalizedText();
			},
			getNormalizedDescription(rowData) {
				return rowData.descriptionText?.getNormalizedText();
			},
		}

	}
</script>
<style src="../Content/menu-list.css"></style>
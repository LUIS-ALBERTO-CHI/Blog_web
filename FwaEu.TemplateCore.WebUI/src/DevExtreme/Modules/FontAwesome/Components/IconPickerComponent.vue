<template>
	<dx-drop-down-button ref="iconPickerButton"
						 dropDownContentTemplate="iconPicker"
						 class="icon-picker-button"
						 stylingMode="text"
						 :icon="selectedIconClass"
						 :drop-down-options="{ width: 'auto' }">
		<template #iconPicker>
			<div class="icon-picker-list-container" :style="{ 'width' : `${containerListMinWidth}px`}">
				<dx-list :data-source="dataSource"
						 :height="400"
						 :search-enabled="true"
						 page-load-mode="scrollBottom">
					<template #item="{ data }">
						<div class="icon-picker-group">
							<div class="icon-picker-item" v-for="icon in data" :key="icon.iconName" @click.stop.prevent="onIconClicked(icon)">
								<i :title="icon.label" :class="icon.iconClass"></i>
							</div>
						</div>
					</template>
				</dx-list>
			</div>
		</template>
	</dx-drop-down-button>
</template>
<script>

	import DxDropDownButton from 'devextreme-vue/drop-down-button';
	import DataSource from "devextreme/data/data_source";
	import DxList from 'devextreme-vue/list';
	import { AsyncLazy } from "@/Fwamework/Core/Services/lazy-load";

	export default {
		components: {
			DxList,
			DxDropDownButton
		},
		props: {
			columnCount: {
				default: 10
			},
			pageSize: {
				default: 20
			},
			modelValue: {
				type: String,
				default: 'fas fa-file-chart-line'
			}
		},
		data() {
			return {
				selectedIconClass: this.modelValue,
				iconList: new AsyncLazy(() => this.initIconsListAsync()),
				dataSource: new DataSource({ paginate: true, pageSize: this.pageSize, load: this.loadIconsAsync })
			};
		},
		methods: {
			iterator(a, ntoTake, current) {
				let end = current + ntoTake;
				var part = a.slice(current, end);
				current = end < a.length ? end : 0;
				return part;
			},
			getGroupedIconByColumnCount(icons, iteratorIcons) {
				let groupedIcons = [];
				const columnCount = parseInt(this.columnCount);
				for (let i = 0; i < iteratorIcons.length; i += columnCount) {
					groupedIcons.push(icons.slice(i, i + columnCount));
				}
				return groupedIcons;
			},
			async loadIconsAsync(loadOptions) {
				let icons = await this.iconList.getValueAsync();
				if (loadOptions.searchValue) {
					const searchValue = loadOptions.searchValue.toLowerCase();
					icons = icons.filter(icon => icon.searchTerms.some(term => term.startsWith(searchValue)));
				}

				let iteratorIcons = this.iterator(icons, loadOptions.take * this.columnCount, loadOptions.skip * this.columnCount);

				const groupedIcons = this.getGroupedIconByColumnCount(icons, iteratorIcons);

				return groupedIcons;
			},
			async initIconsListAsync() {
				const iconList = (await import('@fwaeu/fontawesome-pro/metadata/icons.json')).default;

				let icons = Object.keys(iconList).map(iconName => {
					const metaData = iconList[iconName];
					const iconPrefixes = metaData.styles.map(style => `fa${style[0]}`);
					const searchTerms = metaData.search.terms.map(t => t.toString().toLowerCase());
					searchTerms.unshift(metaData.label.toLowerCase());

					return {
						iconName: iconName,
						iconClass: `${iconPrefixes[0]} fa-${iconName}`,
						label: metaData.label,
						searchTerms: searchTerms,
						styles: iconPrefixes
					};
				});
				return icons;
			},
			onIconClicked(icon) {
				this.selectedIconClass = icon.iconClass;
				this.$emit('update:modelValue', this.selectedIconClass);
				this.$refs.iconPickerButton.instance.close();
			}
		},
		computed: {
			containerListMinWidth() {
				const iconItemMargin = 20;
				const iconItemWidth = 30;
				return (iconItemMargin + iconItemWidth) * this.columnCount;
			}
		}
	}
</script>
<style type="text/css" src="@/Modules/FontAwesome/Components/Content/icon-picker.css"></style>
<style>
	.icon-picker-button .dx-button-content > i:first-child {
		color: var(--secondary-text-color);
	}

	.dx-dropdownbutton-popup-wrapper .icon-picker-list-container .icon-picker-group.dx-list-item-content {
		padding: 0;
	}
</style>
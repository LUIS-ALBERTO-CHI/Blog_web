<template>
	<Dropdown ref="iconPickerButton" :options="icons" v-model="selectedIconClass" optionGroupChildren="items" optionGroupLabel="label"
			  class="border-0 icon-picker-button">
		<template #optiongroup="{ option }">
			<div>
				<InputText class="w-full" @input="initIconsListAsync" v-model="searchValue">
				</InputText>
			</div>
		</template>
		<template #value="icon">
			<div v-if="icon.value" style="font-size: 25px">
				<i :class="selectedIconClass"></i>
			</div>
		</template>
		<template #option="{ option }">
			<div class="icon-picker-group">
				<div class="icon-picker-item" v-for="icon in option" :key="icon.iconName" @click.stop.prevent="onIconClicked(icon)">
					<i :title="icon.label" :class="icon.iconClass"></i>
				</div>
			</div>
		</template>
	</Dropdown>
</template>

<script>
	import Dropdown from 'primevue/dropdown';
	import InputText from 'primevue/inputtext';

	export default {
		components: {
			Dropdown,
			InputText
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
				icons: [],
				searchValue: ""
			};
		},
		async created() {
			await this.initIconsListAsync();
		},
		methods: {
			async initIconsListAsync(e) {
				
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
						styles: iconPrefixes,
					};
				});
				
				if (this.searchValue) {
					const searchValue = this.searchValue.toLowerCase();
					icons = icons.filter(icon => icon.searchTerms.some(term => term.startsWith(searchValue)));
				}

				const groupedIcons = this.getGroupedIconByColumnCount(icons);
				this.icons = groupedIcons;
				this.icons = [{ items: groupedIcons }];

			},
			getGroupedIconByColumnCount(icons) {
				let groupedIcons = [];
				const columnCount = parseInt(this.columnCount);
				for (let i = 0; i < icons.length; i += columnCount) {
					groupedIcons.push(icons.slice(i, i + columnCount));
				}
				return groupedIcons;
			},
			onIconClicked(icon) {
				this.selectedIconClass = icon.iconClass;
				this.$emit('update:modelValue', this.selectedIconClass);
				this.$refs.iconPickerButton.hide();
			}
		}
	};
</script>
<style type="text/css" src="@/Modules/FontAwesome/Components/Content/icon-picker.css"></style>
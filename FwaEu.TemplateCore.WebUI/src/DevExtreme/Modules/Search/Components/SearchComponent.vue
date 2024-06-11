<template>
	<div class="search-header">
		<div class="search-controls">
			<dx-text-box ref="searchSelectBox"
						 v-model="searchValue"
						 placeholder=" "
						 width="100%"
						 @enter-key="performSearchAsync"
						 v-click-outside="hideContent">
				<dx-text-box-button location="after" name="filters" :options="filterButtonOptions" />
			</dx-text-box>

		</div>
		<dx-popover :width="smallModeEnabled ? '95%' : null"
					:max-width="smallModeEnabled ? null : '300px'"
					v-model:visible="showResults"
					target=".search-header"
					:position="resultsPosition">
			<div>
				<dx-list v-if="showResults" ref="searchResultsList"
						 :data-source="searchDataSource"
						 :height="resultsListHeight"
						 page-load-mode="nextButton"
						 @content-ready="updateResultsListSize"
						 @item-click="onResultItemClick"
						 class="search-results-list"
						 :no-data-text="$t('noSearchResults')">
					<template #item="{ data: result }">
						<div class="search-result-item">
							<i :class="result.icon + ' search-result-item-icon'"></i>
							<div>
								<router-link :to="result.link" class="search-result-item-title">
									<text-highlight :query="realSearchValue"
													:caseSensitive="false"
													:diacriticsSensitive="false">
										{{result.text}}
									</text-highlight>
								</router-link>
								<div class="search-result-item-description">
									<text-highlight :query="realSearchValue"
													:caseSensitive="false"
													:diacriticsSensitive="false">
										{{result.description}}
									</text-highlight>
								</div>
							</div>
						</div>
					</template>

				</dx-list>

			</div>
		</dx-popover>

		<dx-context-menu :items="$props.searchItems"
						 display-expr="displayName"
						 @item-click="setSelectedFilter"
						 :width="170"
						 :visible="searchFiltersVisible"
						 :position="menuPositionConfig"
						 css-class="search-filters-menu" 
						 v-click-outside="onClickOutsideContextMenu" />
	</div>
</template>

<script>
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import DxContextMenu from "devextreme-vue/context-menu";
	import DxList from 'devextreme-vue/list';
	import { DxPopover } from 'devextreme-vue/popover';
	import DxTextBox, { DxButton as DxTextBoxButton } from 'devextreme-vue/text-box';
	import { SearchContext } from '@/Modules/Search/Services/search-context';
	import SearchService, { SearchProviderKeySeparator } from '@/Modules/Search/Services/search-service';
	import TextHighlight from 'vue-word-highlighter';

	const resultsListMaxHeight = 400;
	export default {
		components: {
			DxContextMenu,
			DxList,
			DxPopover,
			DxTextBox,
			DxTextBoxButton,
			TextHighlight
		},
		props: {
			smallModeEnabled: Boolean,
			searchItems: {
				required: true,
				type: Array
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/Search/Components/Content/search-messages.*.json'));
		},
		data() {
			const $this = this;
			return {
				context: new SearchContext(this.$router, this.$i18n),
				searchValue: "",
				realSearchValue: "",
				showResults: false,
				searchFiltersVisible: false,
				canLoadMore: false,
				results: [],
				resultsListHeight: resultsListMaxHeight,
				filterButtonOptions: {
					icon: "fas fa-filter-list",
					stylingMode: "text",
					elementAttr: {
						class: "search-filters-button"
					},
					onClick: this.toggleFiltersList
				},
				searchDataSource: {
					load: async () => {
						const searchResult = await SearchService.searchAsync($this.searchValue, $this.context);
						$this.canLoadMore = searchResult.canLoadMore;
						$this.realSearchValue = searchResult.search ?? "";
						$this.showResults = true;
						return searchResult.results;
					},
					paginate: true,
					pageSize: -1//HACK: Because pagination is controlled by SearchService we put -1 in order to always have a value smaller than returned results
				},
				resultsPosition: {
					my: "top center",
					at: "bottom center"
				},
				menuPositionConfig: {
					my: "top left",
					at: "bottom left",
					of: ".search-filters-button"
				}
			};
		},
		mounted() {
			if (this.smallModeEnabled)
				this.$refs.searchSelectBox.instance.focus();
		},
		methods: {
			hideContent() {
				if (this.smallModeEnabled && !this.searchFiltersVisible)
					this.$emit('hide-content');
			},
			onClickOutsideContextMenu() {
				if (this.smallModeEnabled && this.searchFiltersVisible)
					this.searchFiltersVisible = false;
			},
			toggleFiltersList(e) {
				e.event.stopPropagation();
				e.event.preventDefault();
				this.searchFiltersVisible = !this.searchFiltersVisible;
			},
			setSelectedFilter(e) {
				if (e.itemData) {
					this.searchValue = e.itemData.key + SearchProviderKeySeparator;
					this.$refs.searchSelectBox.instance.focus();
					this.canLoadMore = true;
					this.showResults = false;
				}
			},
			async performSearchAsync() {
				this.context.previousSearch = null;
				if (!this.$refs.searchResultsList) {
					this.showResults = true;
				}
				else {
					this.$refs.searchResultsList.instance.reload();
				}
			},
			updateResultsListSize() {
				const currentListHeight = this.$refs.searchResultsList.instance._scrollView._$content[0].clientHeight;
				if (currentListHeight <= resultsListMaxHeight) {
					this.resultsListHeight = null;
				} else {
					this.resultsListHeight = resultsListMaxHeight;
				}
				this.$refs.searchResultsList.$_instance._toggleNextButton(this.canLoadMore);
			},
			onResultItemClick(e) {
				if (e.itemData?.link) {
					this.$emit('hide-content');
					this.searchValue = "";
					this.realSearchValue = "";
					this.showResults = false;
					this.$router.push(e.itemData.link);
				}
			}
		}
	}
</script>
<style type="text/css" src="@/Modules/Search/Components/Content/search.css"></style>

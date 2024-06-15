<template>
	<div class="search-header">
		<div class="search-controls">
			<span class="p-inputgroup">
				<InputText ref="searchSelectBox"
						   v-model="searchValue"
						   placeholder=" "
						   @keydown.enter="performSearchAsync"
						   style="width: auto">
				</InputText>
				<Button icon="fa-solid fa-filter-list" @click="toggleFiltersList" aria-haspopup="true" />
				<ContextMenu ref="menu" :model="$props.searchItems" class="menu">
					<template #item="{ item }">
						<span :command="item.key" @click="setSelectedFilter(item.key)">
							<i :class="item.icon" class="menu-icon"></i>
							<span class="menu-item-tittle">{{ item.displayName }}</span>
						</span>
					</template>
				</ContextMenu>
			</span>
		</div>
		<OverlayPanel ref="op"
					  :max-width="smallModeEnabled ? null : '300px'">
			<div>
				<Listbox v-if="showResults"
						 ref="searchResultsList"
						 :options="results"  
						 class="search-results-list"
						 listStyle="max-height:350px">
					<template #option="{option: result}">
						<div class="search-result-item"  @click="onResultItemClick(result)">
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
				</Listbox>
			</div>
		</OverlayPanel>
	</div>
</template>

<script>
	import { SearchContext } from '@/Modules/Search/Services/search-context';
	import SearchService, { SearchProviderKeySeparator } from '@/Modules/Search/Services/search-service';
	import TextHighlight from 'vue-word-highlighter';
	import InputText from 'primevue/inputtext';
	import Button from 'primevue/button';
	import { ref, onMounted  } from 'vue';
	import ContextMenu from 'primevue/contextmenu';
	import Listbox from 'primevue/listbox';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import OverlayPanel from 'primevue/overlaypanel';

	const resultsListMaxHeight = 400;
	export default {
		components: {
			TextHighlight,
			InputText,
			Button,
			ContextMenu,
			Listbox,
			OverlayPanel
		},
			setup() {
			const searchValue = ref(""); 
			const results = ref([]);
			const canLoadMore = ref(false);
			const realSearchValue = ref("");
			const showResults = ref(false);
			const menu = ref();
			const searchFiltersVisible = ref(false);
			const smallModeEnabled = ref(Boolean);
			const searchSelectBox = ref(null);

			onMounted(() => {
				if (smallModeEnabled.value && searchSelectBox.value) {
				const instance = searchSelectBox.value.instance;
				if (instance && typeof instance.focus === 'function') {
					instance.focus();
				}
				}
			});

			const toggleFiltersList = (event) => {
				menu.value.show(event);
			};

			const setSelectedFilter = (filterKey) => {
			  searchValue.value = filterKey + SearchProviderKeySeparator;
			  searchSelectBox.value.$el.focus();
			  canLoadMore.value = true;
			  showResults.value = false;
			};

			return {
				searchValue,
				results,
				canLoadMore,
				realSearchValue,
				showResults,
				menu,
				toggleFiltersList,
				searchFiltersVisible,
				setSelectedFilter,
				searchSelectBox
			};
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
				resultsListHeight: resultsListMaxHeight,
				searchDataSource: {
					load: async () => {
						const searchResult = await SearchService.searchAsync($this.searchValue, $this.context);
						$this.canLoadMore = searchResult.canLoadMore;
						$this.realSearchValue = searchResult.search ?? "";
						$this.showResults = true;
						return this.results;
					},
					paginate: true,
					pageSize: -1
				},
				resultsPosition: {
					my: "top center",
					at: "bottom center"
				}
			};

		},
		methods: {
			async performSearchAsync(e) {
				this.$refs.op.show(e);
				this.context.previousSearch = null;
				const searchResult = await SearchService.searchAsync(this.searchValue, this.context);
				this.results = searchResult.results;
				this.canLoadMore = searchResult.canLoadMore;
				this.realSearchValue = searchResult.search ?? "";

				if (!this.$refs.searchResultsList) {
					this.showResults = true;
				} else {
					this.$nextTick(() => {
						if (this.$refs.searchResultsList.instance) {
							this.$refs.searchResultsList.instance.reload();
						}
					});
				}
			},
			onResultItemClick(result) {
				if (result?.link) {
					this.$emit('hide-content');
					this.searchValue = "";
					this.realSearchValue = "";
					this.showResults = false;
					this.$router.push(result.link);
					this.$refs.op.hide();
				}
			}
		}
	}
</script>
<style type="text/css" src="@/Modules/Search/Components/Content/search.css"></style>
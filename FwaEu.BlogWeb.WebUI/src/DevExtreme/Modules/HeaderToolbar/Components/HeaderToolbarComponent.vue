<template>
	<div class="header-component">
		<header>
			<div class="header-toolbar">
				<div class="header-item header-item--before">
					<dx-button v-if="menuToggleEnabled"
							   icon="menu"
							   styling-mode="text"
							   @click="toggleMenuFunc"
							   class="menu-button" />
					<div v-if="!!title" class="header-title">
						<router-link :to="{ name: 'default' }" class="header-title-img-label">
							<company-logo-component/>
							{{ formattedTitle }}
						</router-link>
						
					</div>
					<vue-use-online>
					</vue-use-online>
				</div>

				<div v-if="ready" class="header-item header-item--after header-items-container">
					<dx-button v-for="headerItem in itemsToRender"
								:key="headerItem.configuration.key"
								@click="(e) => toggleHeaderItemContent(e, headerItem)"
								class="header-item-container"
								height="100%"
								styling-mode="text">
						<template #content>
							<div class="dx-button-content">
								<component :is="headerItem.configuration.component"
											:fetched-data="headerItem.fetchedData"
											:small-mode-enabled="showSmallMode"
											:is-x-small="isXSmall" :is-large="isLarge"
											:is-small="isSmall" :is-medium="isMedium" />
							</div>
						</template>
					</dx-button>
					<div class="progress-circular-wrapper"></div>
				</div>
			</div>
		</header>
		<div v-show="showHeaderItemContent"
			 class="header-item-content-container">
			<transition name="fade">
				<div v-if="selectedItem">
					<component :is="selectedItem.configuration.smallModeContentComponent"
							   :fetched-data="selectedItem.fetchedData"
							   @hide-content="onHideContent"
							   :is-x-small="isXSmall" :is-large="isLarge"
							   :is-small="isSmall" :is-medium="isMedium" 
							   class="header-item-content"/>
					<div class="header-item-overlay"></div>
				</div>
			</transition>
		</div>
	</div>
</template>

<script>
	import DxButton from "devextreme-vue/button";
	import HeaderService from "@/Modules/Header/Services/header-service";
	import VueUseOnline from "@/Fwamework/OnlineStatus/Components/OnlineStatusIndicator.vue";

	export default {
		props: {
			isXSmall: Boolean,
			isSmall: Boolean,
			isMedium: Boolean,
			isLarge: Boolean,
			menuToggleEnabled: Boolean,
			title: String,
			toggleMenuFunc: Function			
		},
		components: {
			DxButton,
			VueUseOnline
		},
		data() {
			return {
				items: HeaderService.getAllItems(),
				selectedItem: null,
				ready: false,
				visibilityChangedOff: HeaderService.onVisibilityChanged(this.onVisibilityChanged)
			};
		},
		async created() {
			const itemsToBind = this.visibleItems.filter(hi => !!hi.configuration.fetchDataAsync);
			await Promise.all(itemsToBind.map(hi => HeaderService.reloadDataAsync(hi)));
			
			this.ready = true;
		},
        beforeUnmount() {
			this.visibilityChangedOff();
		},
		methods: {

			async onVisibilityChanged(e) {

				if (e.isVisible) {
					await HeaderService.reloadDataAsync(this.items.find(hi => hi.configuration.key == e.key));
				}
				else if (this.selectedItem && this.selectedItem.configuration.key == e.key) {
					this.selectedItem = null;
				}
			},
			toggleHeaderItemContent(args, item) {
				if (this.showSmallMode) {
					args.event.stopPropagation();
					this.selectedItem = this.selectedItem ? null : item;
				} else {
					args.event.preventDefault();
				}
			},
			onHideContent() {
				this.selectedItem = null;
			}
		},
		computed: {
			itemsToRender() {
				return this.visibleItems.filter(vi => !vi.fetchDataAsync || vi.fetchedData);
			},
			visibleItems() {
				return this.items.filter(hi => hi.isVisible);
			},
			showSmallMode() {
				return this.isXSmall;
			},
			showHeaderItemContent() {
				return this.showSmallMode && this.selectedItem;
			},
			formattedTitle() {
				return this.showSmallMode || this.isSmall ? "" : this.title;
			}
		},
		watch: {
			showSmallMode() {
				if (!this.showSmallMode)
					this.onHideContent();
			}
		}
	}
</script>
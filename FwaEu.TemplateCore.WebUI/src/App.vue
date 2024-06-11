<template>
	<div id="app" :class="zoneName">
		<div :class="appCssClasses">
			<component v-if="pageLayout" :is="pageLayout" :title="currentPageTitle">
				<template #content-header>
					<breadcrumbs></breadcrumbs>
				</template>
				<router-view :key="$route.fullPath" />
				<template #footer>
					<application-footer-component />
				</template>
			</component>
		</div>
	</div>

	<!--PrimeVue's singleton global components, If you use DevExtreme, you can remove it-->
	<ConfirmDialog/>
	<DynamicDialog/>
</template>

<script>
	import { defineAsyncComponent, shallowRef, computed } from 'vue'
	import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
	const applicationLayout = Configuration.application.layoutComponent;
	const DefaultPageLayout = defineAsyncComponent(() => import(`./TemplateCore/Components/Layouts/${applicationLayout}.vue`));
	import Breadcrumbs from "@/Fwamework/Breadcrumbs/Components/BreadcrumbsComponent.vue";
	import BreadcrumbService from '@/Fwamework/Breadcrumbs/Services/breadcrumbs-service';
	import ApplicationZoneService from "@/Fwamework/ApplicationZones/application-zone-service";
	import { useScreenSizeInfo } from '@/Fwamework/Utils/Services/screen-size-info';
	import ConfirmDialog from 'primevue/confirmdialog';
	import DynamicDialog from 'primevue/dynamicdialog';

	export default {
		name: 'app',
		components: {
			Breadcrumbs,
			ConfirmDialog,
			DynamicDialog
		},
		setup() {
			const { cssClasses } = useScreenSizeInfo();
			const appCssClasses = computed(() => {
				return ["app"].concat(cssClasses.value);
			});
			return { appCssClasses };
		},
		data() {
			return {
				currentPageTitle: Configuration.application.name,
				zoneName: null,
				pageLayout: null,
				onRouteProcessedOff: BreadcrumbService.onRouteProcessed(this.onRouteProcessed)
			};
		},

		watch: {
			'$route': {
				inmediate: true,
				handler(to, from) {
					this.loadCurrentRouteSetup(to, from);
				}
			},
			currentPageTitle(newPageTitle) {
				//document.title = newPageTitle;
			}
		},
		methods: {
			loadCurrentRouteSetup(to, from) {
				if (!to.matched.length)
					return;

				if (!this.pageLayout || to.meta?.layout || (from.meta?.layout && !to.meta?.layout)) {
					this.pageLayout = shallowRef(to.meta?.layout ?? DefaultPageLayout);
				}
				this.zoneName = ApplicationZoneService.getCurrentZoneName(this);
			},
			getDefaultPageTitle() {
				return Configuration.application.name;
			},
			getRoutePageTitle(route, resolvedNodes) {
				let defaultPageTitle = this.getDefaultPageTitle();

				if (route.meta?.pageTitle || route.meta?.pageTitleKey) {
					return route.meta?.pageTitle || this.$t(route.meta?.pageTitleKey);
				}

				return resolvedNodes.length && resolvedNodes[0]?.text
					? `${resolvedNodes[0]?.text} - ${Configuration.application.name}` : defaultPageTitle;
			},
			onRouteProcessed(resolved) {
				// this.currentPageTitle = this.getRoutePageTitle(this.$route, resolved.breadcrumbNodes);
			}
		},
		mounted() {
			this.loadCurrentRouteSetup(this.$route, {});
		},
		beforeUnmount() {
			this.onRouteProcessedOff();
		}
	}
</script>

<style lang="scss" src="@/TemplateCore/Content/application-styles.scss" />
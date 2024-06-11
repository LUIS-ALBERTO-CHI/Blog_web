<template>
	<div class="loading-panel-container">

		<dx-load-panel ref="loadingPanel"
					   :class="loaderName"
					   :animation="{hide: {duration: 500, type:'fadeOut'}, show:{delay: 200, type:'fadeIn'}}"
					   :position="getLoadingPanelPosition()"
					   :visible="$wait.waiting(loaderNameFilter)"
					   :show-indicator="false"
					   :show-pane="false"
					   :shading="true"
					   message=""
					   shading-color="#f8f8f8"
					   :delay="delay"
					   width="350" height="200" />
		<v-wait :for="loaderNameFilter">
			<template #waiting>

			</template>
		</v-wait>
		<div v-wait:disabled="loaderNameFilter">
			<slot></slot>
		</div>
	</div>
</template>

<script>
	import { DxLoadPanel } from 'devextreme-vue/load-panel';
	import Loader from '@/Fwamework/Core/Content/loader.svg';

	export default {
		components: {
			DxLoadPanel
		},
		props: {
			container: {
				type: String,
				default: null
			},
			loaderName: {
				type: String,
				default: 'modal_loading'
			},
			delay: {
				type: Number,
				default: 0
			},
		},
		data() {
			return {
				indicatorSrc: Loader,
				displayLoadingPanel: false,
				isLoading: false,
				loadingStateDelayInMs: 0,
				resizeObserver: null
			}
		},
		created() {
			this.resizeObserver = new ResizeObserver(() => {
				this.$refs.loadingPanel?.instance.repaint();
			});
			this.resizeObserver.observe(document.body);
		},
		beforeUnmount() {
			this.resizeObserver.unobserve(document.body);
		},
		methods: {
			getLoadingPanelPosition() {
				return { at: 'center', of: this.container || this.$el };
			},
			setLoading(isLoading) {
				this.isLoading = isLoading;
				const $this = this;
				setTimeout(function () {
					$this.displayLoadingPanel = $this.isLoading;
				}, this.loadingStateDelayInMs)
			}
		},
		computed: {
			loaderNameFilter() {
				return this.loaderName + "*";
			}
		}
	}
</script>

<style>
	#small-indicator,
	#medium-indicator,
	#large-indicator {
		position: fixed;
		top: 20%;
		left: 50%;
		z-index: 9999
	}

	.dx-overlay-wrapper.modal_loading.dx-overlay-modal.dx-loadpanel-wrapper {
		z-index: 1600 !important;
	}
</style>
<template>
	<div class="loading-panel-container">
		<BlockUI :blocked="$wait.waiting(loaderNameFilter)" :fullScreen="true" :class="loaderName" />
		<v-wait :for="loaderNameFilter">
			<template #waiting>
				<img src="/loader.gif" class="loader-image" />
			</template>
		</v-wait>
		<div v-wait:disabled="loaderNameFilter">
			<slot></slot>
		</div>
	</div>
</template>

<script>
	import BlockUI from 'primevue/blockui';
	import Loader from '@/Fwamework/Core/Content/loader.svg';

	export default {
		components: {
			BlockUI
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
				loadingStateDelayInMs: 0
			}
		},
		methods: {
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
</style>
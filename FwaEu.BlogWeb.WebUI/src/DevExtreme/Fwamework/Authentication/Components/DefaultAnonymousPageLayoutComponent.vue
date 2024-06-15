<template>
	<div class="anonymous-page-layout">
		<div class="layout-content-container with-footer d-flex">
			<div class="d-flex application-name">
				<img class="logo" alt="" src="@/BlogWeb/Content/logo.png" height="30" />
				<h2>{{ applicationName }}</h2>
			</div>
			<div class="content">
				<slot />
			</div>
			<div class="footer">
				<slot name="footer" />
			</div>
		</div>

		<loading-panel container="window">
		</loading-panel>
	</div>
</template>

<script>
	import DxScrollView from "devextreme-vue/scroll-view";

	import { defineAsyncComponent } from "vue";
	const LoadingPanel = defineAsyncComponent(() => import('@UILibrary/Fwamework/LoadingPanel/Components/LoadingPanelComponent.vue'));
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
	export default {
		components: {
			DxScrollView,
			LoadingPanel
		},
		props: {
			title: String
		},
		data() {
			return {
				applicationName: Configuration.application.name
			}
		}
	}

</script>

<style type="text/css" scoped>

	.anonymous-page-layout {
		width: 100%;
	}

	.anonymous-page-layout .footer {
		position: absolute;
		bottom: 0;
		width: 100%;
	}

	.application-name {
		display: flex;
		align-items: center;
		justify-content: center;
		height: 44px;
		background-color: #FFFFFF !important;
		box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);
	}

	.application-name h2 {
		margin: 0;
		color: var(--secondary-text-color);
		padding-left: 10px;
	}
</style>
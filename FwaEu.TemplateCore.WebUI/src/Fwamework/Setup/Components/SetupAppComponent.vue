<template>
	<div id="app" class="app">
		<page-container type="summary" v-if="!isAuthenticationRequired()">
			<div v-for="setupTask in setupTasks" :key="setupTask.taskName" class="block">
				<component :is="setupTask.createComponent()" :setupTask="setupTask"></component>
			</div>
			<loading-panel container="window">
			</loading-panel>
		</page-container>
		<page-container type="form" v-else class="setup-login">
			<setup-login @logged-in="onLoggedIn"></setup-login>
		</page-container>
	</div>
</template>
<script>
	import SetupTaskProviderService from "@/Fwamework/Setup/Services/setup-tasks-provider-service";
	import LoadingPanel from "@UILibrary/Fwamework/LoadingPanel/Components/LoadingPanelComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import SetupAuthenticationService from '@/Fwamework/Setup/Services/setup-authentication-service';
	import SetupLogin from '@UILibrary/Fwamework/Setup/Components/SetupLoginPageComponent.vue'


	export default {
		components: {
			LoadingPanel,
			PageContainer,
			SetupLogin
		},

		data() {
			return {
				setupTasks: []
			};
		},
		created() {
			if (!this.isAuthenticationRequired()) {
				this.loadTaskProviders();
			}
		},
		methods: {
			loadTaskProviders() {
				let setupTaskProviders = SetupTaskProviderService.getAll();
				this.setupTasks = setupTaskProviders;
			},
			isAuthenticationRequired() {
				return SetupAuthenticationService.isAuthenticationEnabled() &&
					!SetupAuthenticationService.isAuthenticated();
			},
			onLoggedIn() {
				this.loadTaskProviders();
				this.$forceUpdate();
			}
		}
	};
</script>

<style>
	/*
		Styles copiés de application-styles,
		TODO Thomas: faire les styles de setup
		https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4612
	*/
	html,
	body {
		margin: 0px;
		min-height: 100%;
		background-color: #F8F8F8;
		height: 100%;
		overflow-y: auto !important;
	}

	#app {
		height: 100%;
	}

	* {
		box-sizing: border-box;
	}

	.app {
		@import "@UILibrary/Extensions/Themes/generated/variables.base";
		background-color: #F8F8F8;
		display: flex;
		height: 100%;
		width: 100%;
		font-size: 1.0rem;
	}

	p,
	.block {
		margin: 10px 0px 0px 0px;
	}

	.page-container.setup-login {
		margin-top: 50px;
	}
</style>
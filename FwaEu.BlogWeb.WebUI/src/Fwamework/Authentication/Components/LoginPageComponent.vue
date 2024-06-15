<template>
	<div class="login-page">
		<page-container type="form">
			<h3 class="login-box-content">{{$t('welcomeMessage', {applicationName: applicationName})}}</h3>
			<div class="login-box-container">
				<h3>{{$t('title')}}</h3>
				<box class="login-box">
					<div v-for="(l,index) in loginComponents">
						<component :is="l.loginComponent" />
						<div class="block" v-if="userPartsComponents.length">

							<div :key="index" v-for="(c, index) in userPartsComponents.filter(upc => upc.authenticationHandlerKey == l.handlerKey)">
								<component :is="c.component" />
							</div>
						</div>
						<div class="separator">
							<span class="login-separator" v-if="index < loginComponents.length-1" />
						</div>
					</div>
				</box>
			</div>
		</page-container>
	</div>
</template>

<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import { shallowRef } from "vue";
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
	import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';

	export default {
		data() {
			return {
				loginComponents: null,
				userPartsComponents: [],
				applicationName: Configuration.application.name,
			};
		},
		created: showLoadingPanel(async function () {
			this.loginComponents = shallowRef(await AuthenticationService.createLoginComponentAsync());
			const userPartsComponents = [];
			await Promise.all(UserPartsRegistry.getAll()
				.filter(handler => typeof handler.createLoginComponentAsync === 'function')
				.map(async function (userParthandler) {
					const componentInfo = await userParthandler.createLoginComponentAsync();
					componentInfo.authenticationHandlerKey = userParthandler.authenticationHandlerKey;
					if (componentInfo) {
						userPartsComponents.push(componentInfo);
					}
				}));

			this.userPartsComponents = shallowRef(userPartsComponents);

			await loadMessagesAsync(this, import.meta.glob('@/Fwamework/Authentication/Components/Content/login-messages.*.json'));
		}),
		components: {
			Box,
			PageContainer
		}
	};
</script>
<style type="text/css">
	.login-page {
		margin-top: 50px;
		width: 100%;
	}

		.login-page .box {
			max-width: 730px;
			margin: 0px auto;
		}

		.login-box-container{
			text-align:center;
		}

		.login-box{
			width:45%;
		}

	.login-box-content {
		color: var(--secondary-text-color);
		font-weight: bold;
		font-size: 20px;
		text-align: center;
	}

	.login-separator {
		position: relative;
		display: flex;
		justify-content: space-evenly;
		padding-top: 50px;
	}

	.login-separator::before {
		content: '';
		display: block;
		font-size: 0px;
		position: absolute;
		width: 100%;
		top: 26px;
		height: 8px;
		border-radius: 50%;
		background: linear-gradient(180deg, rgba(234,234,234,1) 0%, rgba(218,218,218,0) 100%);
	}


</style>
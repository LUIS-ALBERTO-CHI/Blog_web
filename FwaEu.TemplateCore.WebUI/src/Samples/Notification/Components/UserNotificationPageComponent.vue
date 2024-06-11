<template>
	<page-container type="form">
		<box title="User notification connection">
			<template #title="{title}">
				{{title}}
				<div :class="['user-notification-connection-status', connectionStatus]"></div>
			</template>
			<template #default>
				<div class="form-buttons">
					<dx-button text="Stop connection" icon="clear" type="danger" @click="stopConnectionAsync" />
					<dx-button text="Start connection" icon="info" type="default" @click="startConnectionAsync" />
				</div>
			</template>
		</box>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxButton } from 'devextreme-vue/button';
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import UserNotificationService from "@/Modules/UserNotifications/Services/user-notification-service";

	const connectedStatus = 'Connected';
	const disconnectedStatus = 'Disconnected';

	export default {
		components: {
			Box,
			DxButton,
			PageContainer
		},
		data() {
			return {
				connectionStatus: disconnectedStatus,
				onStartConnectionOff: UserNotificationService.onStarted(this.onStartConnectionAsync),
				onStopConnectionOff: UserNotificationService.onStopped(this.onStopConnectionAsync),
				onNotifiedOff: UserNotificationService.onNotified(this.onNotifiedAsync)
			}
		},
		created: showLoadingPanel(async function () {

			this.connectionStatus = UserNotificationService.getConnectionState();
		}),
		methods: {
			async stopConnectionAsync() {
				await UserNotificationService.stopConnectionAsync();
			},
			async startConnectionAsync() {
				await UserNotificationService.startConnectionAsync();
			},
			async onStopConnectionAsync() {
				this.connectionStatus = disconnectedStatus;
				NotificationService.showError("User notification connection stopped");
			},
			async onStartConnectionAsync() {
				this.connectionStatus = connectedStatus;
				NotificationService.showConfirmation("User notification connection started");
			},
			async onNotifiedAsync(e) {
				NotificationService.showInformation(`User notification "${e.notificationType}" received: ${JSON.stringify(e.model)}`);
			}
		},
		beforeUnmount() {
			this.onStartConnectionOff();
			this.onStopConnectionOff();
			this.onNotifiedOff();

		}
	}
</script>
<style type="text/css">
	.user-notification-connection-status {
		width: 10px;
		height: 10px;
		border-radius: 10px;
		display: inline-block;
		vertical-align: top;
		margin-left: 5px;
		margin-top: 5px;
		background-color: gray;
	}

		.user-notification-connection-status.Connected {
			background-color: limegreen;
		}

		.user-notification-connection-status.Disconnected {
			background-color: red;
		}
</style>
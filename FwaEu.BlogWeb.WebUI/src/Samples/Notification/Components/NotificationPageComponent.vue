<template>
	<page-container type="form">
		<box title="Information">
			<p>
				Use <strong>Information</strong> notification when you want to display to the user a result
				of an action <u>which not alter data</u>, or when you want to push information
				<u>without direct action from him</u> (like auto-refresh of the master-data).
			</p>
			<div class="block">
				<dx-button text="Information" icon="info" type="default" @click="showInformation" />
			</div>
		</box>
		<box title="Confirmation">
			<p>
				Use <strong>Confirmation</strong> notification when you want to display to the user
				a success, <u>after an action from him</u> (like clicking on the "Save" button of a form).
			</p>
			<div class="block">
				<dx-button text="Confirmation" icon="check" type="success" @click="showConfirmation" />
			</div>
		</box>
		<box title="Warning">
			<p>
				Use <strong>Warning</strong> notification when you want to display a result on which the user <u>can act</u>.
			</p>
			<p>
				For example, if a unique key is violated, the server will return an error code
				which will be managed specifically by displaying a warning.
			</p>
			<div class="block sample-warning-button">
				<dx-button text="Warning" icon="warning" type="default" @click="showWarning" />
			</div>
		</box>
		<box title="Error">
			<p>
				Use <strong>Error</strong> notification when there is an unhandled error (local or from server).
			</p>
			<p>
				Mostly, <u>you shouldn't have to use</u> that kind of notification, because the it's the job of the global unhandled errors handling.
			</p>
			<div class="block">
				<dx-button text="Error" icon="clear" type="danger" @click="showError" />
			</div>
		</box>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxButton } from 'devextreme-vue/button';
	import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			Box,
			DxButton,
			PageContainer
		},
		created: showLoadingPanel(async function ()
		{

		}),
		methods: {
			showWarning()
			{
				NotificationService.showWarning("The country 'France' already exists. Please choose another name.");
			},
			showError()
			{
				NotificationService.showError("Current user is not allowed to update this item.");
			},
			showInformation()
			{
				NotificationService.showInformation("The master-data have been updated.");
			},
			showConfirmation()
			{
				NotificationService.showConfirmation("Item successfuly updated.");
			}
		}
	}
</script>
<style type="text/css">
	.sample-warning-button .dx-button-mode-contained
	{
		background-color: #FFA500;
	}

	.sample-warning-button .dx-button-mode-contained.dx-state-hover
	{
		background-color: #F29900;
	}

	.sample-warning-button .dx-button-mode-contained.dx-state-focused
	{
		background-color: #E58D00;
	}

	.sample-warning-button .dx-button-mode-contained.dx-state-active
	{
		background-color: #D88200;
	}
</style>
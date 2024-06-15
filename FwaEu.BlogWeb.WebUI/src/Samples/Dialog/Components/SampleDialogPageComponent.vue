<template>
	<page-container type="summary">

		<box title="Alert">
			<button @click="showAlert">Test</button>

		</box>
		<box title="Confirm">
			<button @click="showConfirmAsync">Test</button>
			Result: {{confirmResult}}
		</box>
	</page-container>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import DialogService from '@UILibrary/Modules/Dialog/Services/dialog-service';


	export default {
		components: {
			Box,
			PageContainer
		},
		data() {
			return {
				confirmResult: false
			};
		},
		methods: {

			showAlert() {
				DialogService.alertAsync("This is a test alert Dialog");
			},
			async showConfirmAsync() {
				this.confirmResult = await DialogService.confirmAsync("Can you confirm?");
			}
		}
	}
</script>
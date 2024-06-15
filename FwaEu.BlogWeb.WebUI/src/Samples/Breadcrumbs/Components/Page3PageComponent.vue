<template>
	<page-container type="form">
		<box title="Bienvenue à Page3 !">
			<p>
				Cliquez sur le bouton pour aller à a page suivante&nbsp;:
				<dx-button @click="goToFourthPage" text="Suivant"></dx-button>
			</p>
		</box>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import DxButton from 'devextreme-vue/button';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue"
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {

		components: {
			Box,
			DxButton,
			PageContainer
		},
		created: showLoadingPanel(async function () {

		}),
		methods: {
			goToFourthPage() {
				this.$router.push({ name: 'Page4' })
			}
		}
	}
</script>
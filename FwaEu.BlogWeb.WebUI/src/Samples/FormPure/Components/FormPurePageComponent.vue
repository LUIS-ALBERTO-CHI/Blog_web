<template>
    <page-container type="form">
		<div style="background-color: yellow;">
      		<h1>FORM</h1>
	  	</div>
    </page-container>
</template>

<script>
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			PageContainer
		},
		created: showLoadingPanel(async function () {

		}),
	}
</script>


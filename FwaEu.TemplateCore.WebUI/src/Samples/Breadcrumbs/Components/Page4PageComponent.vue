<template>
	<page-container type="form">

		<box title="Bienvenue à Page4 !">
			<p>Cette page utilise 'onNodeResolve' pour définir page1 comme son parent.</p>
			<div class="block">
				<code class="sample-code">
					onNodeResolve(node) { <br />
					const name = "Dernier Page"; <br />
					node.parentNode = 'Page1'; <br />
					node.to = '';<br />
					node.text = name;<br />

					return node;<br />
					}<br />
				</code>
			</div>
		</box>

	</page-container>
</template>

<script>

	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue"
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";


	export default {
		components: {
			Box,
			PageContainer
		},
		created: showLoadingPanel(async function () {

		}),

	}
</script>
<style type="text/css" scoped="scoped">
	.sample-code {
		display: block;
		padding-left: 10px;
		border-left: 5px solid #B0CACA;
	}
</style>
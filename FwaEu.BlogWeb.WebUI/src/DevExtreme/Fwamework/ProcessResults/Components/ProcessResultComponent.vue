<template>
	<div class="process-result">
		<slot></slot>
		<div class="block">
			<dx-accordion :data-source="contexts" :collapsible="true" :multiple="true" :deferRendering="false">

				<template #title="tabPanelModel">
					<div>
						{{ tabPanelModel.data.name }}
					</div>
				</template>
				<template #item="tabPanelModel">
					<context :context="tabPanelModel.data"></context>
				</template>
			</dx-accordion>
		</div>
	</div>
</template>
<script>
import Context from '@/Fwamework/ProcessResults/Components/ProcessResultContextComponent.vue';
import { DxAccordion } from 'devextreme-vue/accordion';


export default {
	components: {
		Context,
		DxAccordion
	},
	props: {
		results: {
			type: Object,
			default: () => { }
		}
	},

	computed: {

		contexts() {
			return this.results?.contexts
		}
	},

}
</script>
<style src="@/Fwamework/ProcessResults/Content/process-results.css"></style>
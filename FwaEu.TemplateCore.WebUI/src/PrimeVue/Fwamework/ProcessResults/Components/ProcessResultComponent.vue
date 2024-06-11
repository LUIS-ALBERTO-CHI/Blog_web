<template>
	<div class="process-result">
		<slot></slot>
		<div class="block">
			<Accordion :activeIndex="0"
					   :multiple="true">
					<AccordionTab v-for="(context, index) in contexts" :key="index">
						<template #header>
							{{ context.name }}
						</template>
						<Context :context="context"></Context>
					</AccordionTab>
			</Accordion>
		</div>
	</div>
</template>
<script>
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import Context from '@/Fwamework/ProcessResults/Components/ProcessResultContextComponent.vue';

export default {
	components: {
		Context,
		Accordion,
		AccordionTab
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

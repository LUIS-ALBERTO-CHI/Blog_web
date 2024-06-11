<template>
	<div class="process-result-context">
		<div v-if="context.entries">
			<entry :key="index" v-for="(entry, index) in context.entries" :entry="entry"></entry>
		</div>
	</div>
</template>
<script>
	import Entry from '@/Fwamework/ProcessResults/Components/ProcessResultContextEntryComponent.vue';
	

	export default {
		components: {
			Entry
		},
		props: {
			context: {
				type: Object,
				default: () => { }
			}
		}
	}
</script>
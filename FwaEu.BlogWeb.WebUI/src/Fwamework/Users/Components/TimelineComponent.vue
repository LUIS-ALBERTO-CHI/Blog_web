<template>
	<div class="timeline">
		<div v-for="item in items" :key="item.text" class="timeline-item">
			<span class="timeline-label">{{ item.text }}</span>
			<span class="timeline-data">
				<user-date :date="item.data.date" :userId="item.data.userId" />
			</span>
		</div>
	</div>
</template>

<script>
	import UserDate from "@/Fwamework/Users/Components/UserDateComponent.vue"

	export default {
		components: {
			UserDate
		},
		props: {
			items: {
				type: Array,
				required: true
			}
		}
	}

</script>
<style type="text/css" src="./Content/timeline.css" />
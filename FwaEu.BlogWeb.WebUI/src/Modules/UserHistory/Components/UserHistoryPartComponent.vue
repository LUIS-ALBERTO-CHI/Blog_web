<template>
	<div>
		<timeline :items="getHistoryEntries()" />
	</div>
</template>
<script>
	import Timeline from '@/Fwamework/Users/Components/TimelineComponent.vue';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	export default {
		components: {
			Timeline
		},
		props: {
			modelValue: {
				type: Object,
				default: () => { }
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('@/Modules/UserHistory/Components/Content/user-history-part-messages.*.json'));
		},
		methods: {
			getHistoryEntries() {
				let historyEntries = [];
				if (this.modelValue.data.createdOn || this.modelValue.data.createdById) {
					historyEntries.push({
						text: this.$t('creation'),
						data: {
							date: this.modelValue.data.createdOn ? new Date(this.modelValue.data.createdOn) : null,
							userId: this.modelValue.data.createdById
						}
					});
				}
				if (this.modelValue.data.updatedOn || this.modelValue.data.updatedById) {

					historyEntries.push(
					{
						text: this.$t('modification'),
						data: {
							date: this.modelValue.data.updatedOn ? new Date(this.modelValue.data.updatedOn) : null,
							userId: this.modelValue.data.updatedById
						}
					});
				}
				return historyEntries;
			}
		}
	}
</script>
<template>
	<persistent-notifications :notifications-list="notificationsList"
							  :is-small-mode="true" 
							  @hide-content="hideContent"/>
</template>

<script>
	import PersistentNotifications from "@UILibrary/Modules/PersistentNotifications/Components/PersistentNotificationsComponent.vue";

	export default {
		components: {
			PersistentNotifications
		},
		props: {
			fetchedData: {
				required: true,
				type: Object
			}
		},
		data() {
			return {
				notificationsList: this.$props.fetchedData.notificationsList
			};
		},
		methods: {
			hideContent() {
				this.$emit('hide-content');
			},
		}
	}
</script>
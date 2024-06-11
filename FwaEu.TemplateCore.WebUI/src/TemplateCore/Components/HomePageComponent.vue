<template>
	<page-container type="summary">
		<user-tasks-list v-model="userTasks" :display-zone-name="userTasksDisplayZoneName" />
	</page-container>
</template>
<script>

	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
    import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import UserTasksList from '@/Modules/UserTasksList/Components/UserTasksListComponent.vue';
	import UserTaskListDisplayZoneHandler, { ListDisplayZoneName } from "@/Modules/UserTasksList/Services/user-task-display-zone-handler";

	export default {
		components: {
			PageContainer,
			UserTasksList
		},
		data() {
			return {
				userTasks: [],
				userTasksDisplayZoneName: ListDisplayZoneName
			};
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('./Content/home-page-messages.*.json'));
			this.userTasks = await UserTaskListDisplayZoneHandler.getAllTasksAsync();
		}
	}
</script>
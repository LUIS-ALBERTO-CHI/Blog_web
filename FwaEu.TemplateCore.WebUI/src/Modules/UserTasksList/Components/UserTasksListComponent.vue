<template>
	<box class="user-tasks-list-container" :title="titleToDisplay">
		<div class="user-task-item-container" v-for="(userTask, index) in userTasks" :key="index">
			<slot name="userTaskItem">
				<user-task-item v-model="userTasks[index]" :display-zone-name="displayZoneName" />
			</slot>
		</div>
	</box>

</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import UserTaskItem from "./UserTaskItemComponent.vue";

	export default {
		components: {
			Box,
			UserTaskItem
		},
		props: {
			modelValue: {
				type: Array,
				required: true
			},
			title: {
				type: String,
				required: false
			},
			displayZoneName: {
				type: String,
				required: true
			}
		},
		data() {
			return {
				userTasks: this.modelValue

			};
		},
		watch: {
			modelValue() {
				this.userTasks = this.modelValue;
			}
		},
		computed: {
			titleToDisplay() {
				return this.title || this.$t('title');
			}
		}
	}
</script>
<style>
	.user-task-item-container {
		min-width: 200px;
		display: inline-block;
		padding: 20px 0;
	}

		.user-task-item-container + .user-task-item-container {
			margin-left: 20px;
		}
</style>
<i18n>
	{
	"fr": {
	"title": "Actions attendues"
	},
	"en": {
	"title": "Expected actions"
	}
	}
</i18n>
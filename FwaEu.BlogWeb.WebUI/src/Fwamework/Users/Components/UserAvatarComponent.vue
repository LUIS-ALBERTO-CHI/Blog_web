<template>
	<avatar context="user-avatar" :item="userItem" :size="size"></avatar>
</template>
<script>
	import Avatar from '@/Fwamework/Avatar/Components/AvatarComponent.vue';
	import UserFormatterService from '@/Fwamework/Users/Services/user-formatter-service';

	export default {
		components: {
			Avatar
		},
		props: {
			user: {
				type: Object,
				required: true,
				validator: function (model) {
					return model.fullName && (model.avatarUrl || (model.id && model.firstName && model.lastName));
				}
			},
			size: {
				type: String,
				default: "medium",
				validator: function (value) {
					return ["small", "medium", "large", "x-large"].includes(value);
				}
			}
		},
		computed: {
			userItem() {
				return {
					initials: UserFormatterService.generateInitials(this.user),
					...this.user
				}
			}
		}
	}
</script>

<template>
	<span class="user-date" ref="span">
		<date-literal v-if="isDateLiteralVisible" :date="date" display-format="short" :use-tooltip="!hasUser" />
		<span class="user-date-user" v-if="hasUser">
			<user-avatar v-if="userAvatarModel" :user="userAvatarModel" :size="userAvatarSize" />
			<user-tooltip ref="tooltip" :user-id="userId" :user="userAvatarModel" :date="dateForTooltip" />
		</span>
	</span>
</template>

<script>
	import UserTooltip from "@UILibrary/Fwamework/Users/Components/UserTooltipComponent.vue";
	import UserAvatar from "@/Fwamework/Users/Components/UserAvatarComponent.vue";
	import DateLiteral from '@UILibrary/Fwamework/Utils/Components/DateLiteralComponent.vue';
	import UsersMasterDataService from "@/Modules/UserMasterData/Services/users-master-data-service";

	const everywhereDisplayMode = "everywhere";
	const tooltipOnlyDisplayMode = "tooltipOnly";
	const componentOnlyDisplayMode = "componentOnly";

	export default {
		components: {
			UserTooltip,
			UserAvatar,
			DateLiteral
		},
		data() {
			return {
				userAvatarModel: this.user
			}
		},
		created() {
			const $this = this;
			this.$nextTick(async function () {
				if (!$this.userAvatarModel && $this.userId) {
					const masterData = await UsersMasterDataService.getAsync($this.userId);
					const model = {
						id: masterData.id,
						firstName: masterData.firstName,
						lastName: masterData.lastName,
						fullName: masterData.fullName,
						avatarUrl: masterData.avatarUrl
					};

					$this.userAvatarModel = model;
				}

				if ($this.userAvatarModel && $this.$refs.tooltip)
					$this.$refs.tooltip.target = $this.$refs.span;
			});
		},
		props: {
			date: { type: Date, required: false },
			userId: { type: Number, required: false },
			user: { type: Object, required: false },
			userAvatarSize: {
				type: String,
				default: "medium",
				validator: function (value) {
					return ["small", "medium", "large", "x-large"].includes(value);
				}
			},
			dateDisplayMode: {
				type: String,
				default: everywhereDisplayMode,
				validator: (value) => {
					return [everywhereDisplayMode, tooltipOnlyDisplayMode, componentOnlyDisplayMode].includes(value);
				}
			}
		},
		computed: {
			isDateLiteralVisible() {
				return this.dateDisplayMode === everywhereDisplayMode || this.dateDisplayMode === componentOnlyDisplayMode;
			},
			dateForTooltip() {
				return this.dateDisplayMode === everywhereDisplayMode || this.dateDisplayMode === tooltipOnlyDisplayMode
					? this.date
					: null;
			},
			hasUser() {
				return !!(this.userAvatarModel || this.userId);
			}
		}
	}
</script>
<style type="text/css" src="./Content/user-date.css"></style>
<template>
	<page-container type="form">
		<box title="Test user tool tip">
			<div class="block">
				<dx-button ref="tooltipButton"
						   icon="tips"
						   text="UserId + Date"
						   @content-ready="onContentReady" />
				<user-tooltip ref="tooltip" :user-id="userId" :date="new Date()"></user-tooltip>
			</div>
			<div class="block">
				<dx-button ref="tooltipButton2"
						   icon="tips"
						   text="UserId only"
						   @content-ready="onContentReady2" />
				<user-tooltip ref="tooltip2" :user-id="userId"></user-tooltip>
			</div>
		</box>
		<box title="Test user date">
			<div class="block">
				<strong>Date only:</strong>&nbsp;
				<user-date :date="new Date()" />
			</div>
			<div class="block">
				<strong>Date and user:</strong>&nbsp;
				<user-date :date="new Date()" :userId="1" />
			</div>
			<div class="block">
				<strong>Date and user + user tooltip only:</strong>&nbsp;
				<user-date :date="new Date()" :userId="1" dateDisplayMode="componentOnly" />
			</div>
			<div class="block">
				<strong>User only and user+date tooltip:</strong>&nbsp;
				<user-date :date="new Date()" :userId="1" dateDisplayMode="tooltipOnly" userAvatarSize="large"/>
			</div>
		</box>
	</page-container>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import DxButton from "devextreme-vue/button";
	import UserTooltip from "@UILibrary/Fwamework/Users/Components/UserTooltipComponent.vue"
	import UserDate from "@/Fwamework/Users/Components/UserDateComponent.vue"
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";

	export default {
		components: {
			Box,
			UserTooltip,
			UserDate,
			PageContainer,
			DxButton
		},
		data()
		{
			return {
				userId: 1
			};
		},

		methods: {
			onContentReady()
			{
				this.$nextTick(function ()
				{
					this.$refs.tooltip.target = this.$refs.tooltipButton.$el;
				});
			},
			onContentReady2()
			{
				this.$nextTick(function ()
				{
					this.$refs.tooltip2.target = this.$refs.tooltipButton2.$el;
				});
			}
		}
	}
</script>

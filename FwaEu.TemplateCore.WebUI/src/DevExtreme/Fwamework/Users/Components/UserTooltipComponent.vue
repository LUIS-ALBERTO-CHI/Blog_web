<template>
	<div>
		<dx-tooltip ref="toolTipAction" 
					v-model:target="target" 
					width="350px" 
					class="user-tooltip-container"
					@hiding="onTooltipHiding" 
					@showing="onTooltipShowing">
			<user-tooltip-content-component :userData="userData" :date="date" 
											@mouseenter="keepTooltipOpen"
											@hideTooltip="hideTooltip" />
		</dx-tooltip>
	</div>
</template>

<script>
import { DxTooltip } from 'devextreme-vue/tooltip';
import UserTooltipContentComponent from '@/Fwamework/Users/Components/UserTooltipContentComponent.vue';
import UsersMasterDataService from '@/Modules/UserMasterData/Services/users-master-data-service';
import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';

export default {
	components: {
		DxTooltip,
		UserTooltipContentComponent,
	},
	props: {
		user: Object,
		userId: Number,
		date: Date,
	},
	data() {
		return {
			userData: this.user,
			userLazy: new AsyncLazy(() => UsersMasterDataService.getAsync(this.userId)),
			keepOpen: false,
			target: null,
		};
	},
	methods: {
		keepTooltipOpen() {
			this.keepOpen = true;
		},
		hideTooltip() {
			this.keepOpen = false;
			this.$refs.toolTipAction.instance.hide();
		},
		onTooltipHiding(e) {
			if (this.keepOpen) {
				e.cancel = true;
			}
		},
		async onTooltipShowing() {
			if (!this.userData) {
				this.userData = await this.userLazy.getValueAsync();
			}
		},
	},
};
</script>
<style type="text/css" src="@/Fwamework/Users/Components/Content/user-tooltip.css"></style>
<template>
	<div>
		<OverlayPanel ref="opTooltip"
					@mouseenter="() => (tooltipHovered = true)"
					@mouseleave="() => (tooltipHovered = false)"
					class="p-3">
			<UserTooltipContentComponent :userData="userData" 
										:date="date"
										@mouseenter="keepTooltipOpen"
										@hideTooltip="hideTooltip" />
		</OverlayPanel>
	</div>
</template>
<script>
import OverlayPanel from 'primevue/overlaypanel';
import UserTooltipContentComponent from '@/Fwamework/Users/Components/UserTooltipContentComponent.vue';
import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
import UsersMasterDataService from '@/Modules/UserMasterData/Services/users-master-data-service';

export default {
	components: {
		OverlayPanel,
		UserTooltipContentComponent
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
			tooltipHovered: false,
			tooltipHideTimoutId: null,
			target: null,
			keepOpen: false
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
		showTooltip(e) {
			clearTimeout(this.tooltipHideTimoutId);
			this.tooltipHideTimoutId = null;
			this.$refs.opTooltip.hide();
			this.$nextTick(() => {
				this.$refs.opTooltip.show(e);
			});
		},
		async hideTooltip(e) {
			if (!this.userData) {
				this.userData = await this.userLazy.getValueAsync();
			}
			this.tooltipHideTimoutId = setTimeout(() => {
				if (!this.tooltipHovered) {
					this.$refs.opTooltip.hide();
				} else {
					this.hideTooltip(e);
				}
			}, 300);
		},
	},
	watch: {
		target() {
			this.target.removeEventListener("mouseenter", this.showTooltip)
			this.target.removeEventListener("mouseleave", this.hideTooltip)
			this.target.addEventListener("mouseenter", this.showTooltip)
			this.target.addEventListener("mouseleave", this.hideTooltip)
		}
	}
}
</script>
<style type="text/css" src="@/Fwamework/Users/Components/Content/user-tooltip.css"></style>

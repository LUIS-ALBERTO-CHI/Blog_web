<template>
	<div>
		<div v-if="userData" class="user-tooltip" 
			@mouseenter="keepTooltipOpen" 
			@mouseleave="hideTooltip">
			<div class="user-tooltip-avatar">
				<user-avatar :user="userData" size="large"></user-avatar>
			</div>
			<div class="user-tooltip-date" v-if="date">
				<i class="fa fa-regular fa-clock" />
				{{ $d(date, 'numericLong') }}
			</div>
			<div class="user-tooltip-user-details">
				<strong class="full-name">{{ userData.fullName }}</strong>
				<!-- TODO: Implementar el display de datos confidenciales -->
				<!--<a href="mailto:todo@fwa.eu">todo@fwa.eu</a>-->
			</div>
		</div>
		<user-loader-tooltip v-else />
	</div>
</template>

<script>
import UserAvatar from '@/Fwamework/Users/Components/UserAvatarComponent.vue';
import UserLoaderTooltip from '@/Fwamework/Users/Components/UserLoaderTooltip.vue';

export default {
	components: {
		UserAvatar,
		UserLoaderTooltip,
	},
	props: {
		userData: Object,
		date: Date,
	},
	  methods: {
		keepTooltipOpen() {
			this.$emit('mouseenter');
		},
		hideTooltip() {
			this.$emit('hideTooltip');
		},
	},
};
</script>
<style type="text/css" src="@/Fwamework/Users/Components/Content/user-tooltip.css"></style>

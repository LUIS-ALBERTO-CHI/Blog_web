<template>
	<div id="online-status-container"><sup><span :class="showOnlineStatus" class="fa-sharp fa-solid fa-circle"></span></sup></div>
</template>

<script>
	import OnlineService from '@/Fwamework/OnlineStatus/Services/online-service';	

	export default {
		computed: {
			showOnlineStatus() {
				return OnlineService.isOnline() ? "online-status" : "offline-status";
			}
		}	
	}
</script>

<style src="./Content/online-status.css">
</style>
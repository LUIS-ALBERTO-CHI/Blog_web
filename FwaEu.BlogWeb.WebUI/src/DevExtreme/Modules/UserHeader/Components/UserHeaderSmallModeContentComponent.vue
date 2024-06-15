<template>
	<dx-list class="user-header-list"
				@item-click="onItemClick"
				:items="userMenuItems" 
				v-click-outside="hideContent"/>
</template>

<script>
	import DxList from "devextreme-vue/list";

	export default {
		components: {
			DxList
		},
		props: {
			fetchedData: {
				required: true,
				type: Object
			}
		},		
		data() {
			return {
				user: this.fetchedData.currentUser,
				userMenuItems: this.fetchedData.userMenuItems
			};
		},
		methods: {
			hideContent() {
				this.$emit('hide-content');
			},
			onItemClick(e) {
				if (e.itemData.path) {
					this.$router.push(e.itemData.path);
				}
			}
		}
	}
</script>
<style>
		.dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item .dx-icon {
			color: var(--secondary-text-color);
		}
</style>
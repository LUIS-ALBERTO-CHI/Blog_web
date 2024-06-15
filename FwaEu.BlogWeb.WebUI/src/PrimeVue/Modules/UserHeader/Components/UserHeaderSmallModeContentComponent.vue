<template>
	<Listbox  :options="userMenuItems" optionLabel="text" v-click-outside="hideContent">
		<template #option="menuItemOption">
			<div>				
				<i class="menu-option-small-icon" :class="menuItemOption.option.icon"></i><span class="menu-option-small" @click="onItemClick(menuItemOption.option)">{{menuItemOption.option.text}}</span>
			</div>
		</template>
	</Listbox>
</template>

<script>

	import Listbox from 'primevue/listbox';

	export default {
		components: {
			Listbox
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
				if (e.path) {
					this.$router.push(e.path);
				} else {
					if (typeof e.onClick === "function") {
						e.onClick();
					}
				}
				this.hideContent();
			}
		}
	}

</script>

<style >
	.p-listbox .p-listbox-list .p-listbox-item {
		display: flex;
	}
	.p-listbox .p-listbox-list .p-listbox-item > div {
		display: flex;
		flex-grow: 1;
	}
	.menu-option-small {
		flex-grow: 1;
	}
	.menu-option-small-icon {
		color: var(--secondary-text-color);
		width: 24px;
	}

</style>
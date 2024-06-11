<template>
	<div class="action-menu-wrapper">
		<div v-if="displayOutOfgroup">
			<div v-for="(item, index) in exteriorItems" :key="index">

				<Button 
						@click="onActionButtonClick($event, item)"
						:icon="item.icon"
						class="p-button-secondary p-button-text "
						:style="{color: getActionButtonColor(item.color)}"
						v-tooltip.right="item.text" />
			</div>
		</div>

		<div v-if="showMenu">
			<Button
					type="button" 
					class="p-button-text p-button-secondary" 
					@click="displayActionMenu" 
					:icon="showMenuIcon" />

			<Menu ref="primemenu"
				  :model="groupedItems"
				  :popup="true"
				  :class="{'action-menu-left': menuDisplayDirection == 'left' }"/>

		</div>
		<div v-else v-for="(item, index) in groupedItems" :key="index">
			<Button 
					@click="onActionButtonClick($event, item)"
					:icon="item.icon"
					class="p-button-secondary p-button-text"
					:style="{color: getActionButtonColor(item.color)}"
					v-tooltip.right="item.text" />
		</div>
	</div>

</template>
<script>
	import Button from 'primevue/button';
	import Menu from 'primevue/menu';
	import Tooltip from 'primevue/tooltip';

	export default {
		directives: {
			'tooltip': Tooltip
		},
		components: {
			Menu,
			Button,
			Tooltip
		},
		
		props: {
			items: {
				type: Array,
				default: () => [],
				validator: function (items) {
					let itemWithoutText = items.filter(item => (typeof item.text === 'undefined' || item.text === ''));
					return (itemWithoutText.length === 0);

				}
			},
			menuDisplayDirection: {
				type: String,
				default: 'left',
				validator: function (value) {
					return ['right', 'left'].indexOf(value) !== -1;
				}
			},	
			forceMenuMode: {
				type: Boolean,
				default: true
			},
			showMenuIcon: {
				type: String,
				default: 'fa-light fa-ellipsis-vertical'
			},
			maxActionButtons: {
				type: Number,
				default: 2
			}
		},
		
		methods: {
			getActionButtonColor(color) {
				return color ?? 'var(--secondary-text-color)';
			},
			displayActionMenu(event) {
				this.$refs.primemenu.toggle(event);
			},
			
			onActionButtonClick(e, item) {
				let action = item.action;
				this.triggerActionMenuClick(action);
			},
			isPathAbsolute(path) {
				return /^(?:\/\/|[a-z]+:\/\/)/.test(path);
			},
			triggerActionMenuClick(action) {
				if (typeof action !== 'undefined') {
					if (typeof action === "function") {
						action();
					}
					else {
						if (this.isPathAbsolute(action)) {
							window.open(action, '_blank');
						}
						else {
							this.$router.push(action);
						}
					}
				}
			},
			generateCommandFromAction(action) {
				if (typeof action !== 'undefined') {
					if (typeof action === "function") {
						return () => { action() };
					}
					else {
						if (this.isPathAbsolute(action)) {
							return () => { window.open(action, '_blank') };
						}
						else {
							return () => { this.$router.push(action) };
						}
					}
				}
			}
		},
		computed: {
			menuItems() {


				const itemsMenu =  this.items.map((x) => {
					return {
						label: x.text,
						action: x.action,
						command: this.generateCommandFromAction(x.action),
						...x
					};
				});

				return itemsMenu;
			},

			getCommand(item) {
				if (item.action) {

				}
			},
			showMenu() {
				return (this.forceMenuMode && this.menuItems.length > 0) || this.menuItems.length > this.maxActionButtons;
			},		
			displayOutOfgroup() {
				return this.menuItems.some(x => x.outOfGroup);
			},
			exteriorItems() {
				return this.menuItems.filter(x => x.outOfGroup);
			},
			groupedItems() {
				return this.menuItems.filter(x => !x.outOfGroup);
			}
		}
	}
</script>

<style >
	.action-menu-left {
		transform: translate(-100%,0%) translate(38px);	
	}	
</style>
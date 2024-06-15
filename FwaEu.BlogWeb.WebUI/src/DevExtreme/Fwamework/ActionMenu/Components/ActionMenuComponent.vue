<template>
	<div class="action-menu-wrapper">
		<div v-if="displayOutOfgroup">
			<div v-for="(item, index) in exteriorItems" :key="index">
				<dx-button ref="exteriorButton"
						   @click="onActionButtonClick($event, item)"
						   styling-mode="text"
						   :icon="item.icon"
						   :disabled="item.disabled"
						   @content-ready="onContentReadyActionButton"
						   :style="{color: getActionButtonColor(item.color)}" />

				<dx-tooltip v-if="exteriorActionButtons && exteriorActionButtons.length > 0" :target="exteriorActionButtons[index].$el">
					{{item.text}}
				</dx-tooltip>
			</div>
		</div>

		<div v-if="showMenu">
			<dx-button ref="multiItemsActionButton"
					   :icon="showMenuIcon"
					   @click="displayActionMenu"
					   @content-ready="onContentReadyMultiItem" />
			<DxContextMenu ref="actionMenu"
						   :data-source="groupedItems"
						   :target="multiItemsActionButton"
						   :position="menuPosition"
						   :show-event="contextMenuShowEvent"
						   @item-click="itemClick" />
		</div>
		<div v-else v-for="(item, index) in groupedItems" :key="index">
			<dx-button ref="actionButton"
					   @click="onActionButtonClick($event, item)"
					   styling-mode="text"
					   :icon="item.icon"
					   :disabled="item.disabled"
					   @content-ready="onContentReadyActionButton"
					   :style="{color: getActionButtonColor(item.color)}"
					   class="action-button-icon" />

			<dx-tooltip v-if="actionButtons.length > 0"
						ref="toolTipAction"
						:close-on-outside-click="false"
						:target="actionButtons[index].$el">
				{{item.text}}
			</dx-tooltip>
		</div>
	</div>


</template>
<script>
	import DxContextMenu from 'devextreme-vue/context-menu';
	import DxButton from "devextreme-vue/button";
	import { DxTooltip } from 'devextreme-vue/tooltip';
	export default {
		components: {
			DxButton,
			DxTooltip,
			DxContextMenu
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
				default: 'overflow'
			},
			maxActionButtons: {
				type: Number,
				default: 2
			}
		},
		data() {
			return {
				orientation: 'vertical',
				actionButtons: [],
				exteriorActionButtons: [],
				multiItemsActionButton: null,
				contextMenuShowEvent: null //NOTE: Force null in order to prevent default behavior with right click
			};
		},
		methods: {
			getActionButtonColor(color) {
				return color ?? 'var(--secondary-text-color)';
			},
			displayActionMenu(e) {
				e.event.preventDefault();
				e.event.stopPropagation();
				this.$refs.actionMenu.instance.show();
			},
			onContentReadyActionButton() {
				this.$nextTick(() => {
					this.actionButtons = this.$refs.actionButton ?? [];
					this.exteriorActionButtons = this.$refs.exteriorButton ?? [];
				});
			},
			onContentReadyMultiItem() {
				this.$nextTick(() => {
					this.multiItemsActionButton = this.$refs.multiItemsActionButton.$el;
				});
			},
			itemClick(e) {
				if (!e.itemData.disabled) {
					let action = e.itemData.action;
					this.triggerActionMenuClick(action);
				}
			},
			onActionButtonClick(e, item) {
				e.event.stopPropagation();
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
			}
		},
		computed: {
			showMenu() {
				return (this.forceMenuMode && this.groupedItems.length > 1) || this.groupedItems.length > this.maxActionButtons;
			},
			menuPosition() {
				if (this.menuDisplayDirection === 'left') {
					return { at: 'bottom', my: 'right top', offset: '17 0' };//NOTE: 17 is the trigger button size for the current DevExtreme theme
				}
				else {
					return { at: 'bottom left' };
				}
			},
			displayOutOfgroup() {
				return this.items.some(x => x.outOfGroup);
			},
			exteriorItems() {
				return this.items.filter(x => x.outOfGroup);
			},
			groupedItems() {
				return this.items.filter(x => !x.outOfGroup);
			}
		}
	}
</script>
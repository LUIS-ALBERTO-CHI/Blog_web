<template>
	<div class="side-navigation-menu" @click="forwardClick">
		<slot />
		<div class="menu-container">
			<div v-for="menuItem in menuItems" :key="menuItem.text">
				<router-link v-if="!menuItem.items" :to="menuItem.path"
							 :class="[menuItem.selectedClass, 'menu-item--item parent-item']">
					<notification-counter-badge v-if="getShowMenuCounter(menuItem)"
												:title="menuItem?.userTask?.title || ''"
												:value="menuItem?.userTask?.count"
												:minDisplay="menuItem?.userTask?.minDisplay"
												:maxCount="menuItem?.userTask?.maxCount"
												:params="menuItem?.userTask?.params">
					</notification-counter-badge>
					<i :class="`${menuItem.icon} dx-icon`" :style="{color: menuItem.color}" />

					<span>{{menuItem.text}}</span>
					<div v-if="menuItem.menuActionOptions"
						 class="action-menu-container">
						<action-menu :items="menuItem.menuActionOptions.items"
									 menu-display-direction="right"
									 :force-menu-mode="true"
									 :show-menu-icon="getShowMenuIcon(menuItem.menuActionOptions)"></action-menu>
					</div>
				</router-link>

				<div v-else
					 :class="[{ 'group-expanded' : menuItem.expanded }, menuItem.selectedClass, 'menu-item--group-container']"
					 @click="toggleGroupMenuVisibility(menuItem)">
					<div class="menu-item--item parent-item">
						<notification-counter-badge v-if="getShowMenuCounter(menuItem)"
													:value="menuItem?.userTask?.count"
													:minDisplay="menuItem?.userTask?.minDisplay"
													:maxCount="menuItem?.userTask?.maxCount"
													:params="menuItem?.userTask?.params">
						</notification-counter-badge>
						<i :class="`${menuItem.icon} dx-icon`" :style="{ color: menuItem.color }" />
						<span>{{menuItem.text}}</span>
						<i :class="[getExpandIconClass(menuItem.expanded), 'expand-menu-icon fal fa-caret-down']" />
					</div>
					<div @click="handleItemClick" class="menu-item--children-group">
						<router-link :to="item.path" v-for="(item, index) in menuItem.items" :key="getMenuItemKey(item.path)+index"
									 :class="[item.selectedClass, 'menu-item--item child-item']">
							<notification-counter-badge v-if="getShowMenuCounter(item)"
														:title="item?.userTask?.title || ''"
														:value="item?.userTask?.count"
														:minDisplay="item?.userTask?.minDisplay"
														:maxCount="item?.userTask?.maxCount"
														:params="item?.userTask?.params">
							</notification-counter-badge>
							<i :class="`${item.icon} dx-icon small-icon`" />
							<span>{{item.text}}</span>
						</router-link>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import NavigationMenuHelper from '@/Fwamework/NavigationMenu/Services/navigation-menu-helper-service';
	import BreadcrumbsService from "@/Fwamework/Breadcrumbs/Services/breadcrumbs-service";
	import ResolveContext from "@/Fwamework/Breadcrumbs/Services/resolve-context";
	import ActionMenu from "@UILibrary/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	import NotificationCounterBadge from "@/Fwamework/NotificationCounterBadge/Components/NotificationCounterBadgeComponent.vue";

	export default {
		components: {
			ActionMenu,
			NotificationCounterBadge
		},
		props: {
			items: Array,
			compactMode: Boolean
		},
		data() {
			return {
				menuItems: [],
			};
		},
		created() {
			this.menuItems = this.items;
		},
		methods: {
			forwardClick(...args) {
				this.$emit("click", args);
			},

			handleItemClick(e) {
				if (!e.path || this.compactMode) {
					return;
				}
				e.stopPropagation();
			},
			async setSelectedItemStyleAsync(forceUpdate = false) {
				const context = new ResolveContext(this.$router, this.$i18n);
				const resolvedNodes = await BreadcrumbsService.processRouteAsync(this.$route, context);
				NavigationMenuHelper.refreshMenuItems(this.items, this.$router, resolvedNodes);

				if (forceUpdate) {
					this.$forceUpdate();
				}
			},
			toggleGroupMenuVisibility(menuItem) {
				menuItem.expanded = !menuItem.expanded;
				this.$forceUpdate();
			},
			getExpandIconClass(isExpanded) {
				return isExpanded ? 'fa-caret-down--opened' : '';
			},
			getMenuItemKey(path) {
				return path?.name
					? `/${path?.params?.invariantId ?? path.name}`
					: path;
			},
			getShowMenuIcon(menuActionOptions) {
				return menuActionOptions.showMenuIcon ?? 'fa-solid fa-plus';
			},
			getShowMenuCounter(menuItem) {
				const $this = this;
				return (menuItem.userTask)
					||
					(menuItem.items && menuItem.items.some(item => $this.getShowMenuCounter(item)));
			}
		},
		watch: {
			async items() {
				this.menuItems = this.items;
				await this.setSelectedItemStyleAsync(true);
			},
			async $route() {
				await this.setSelectedItemStyleAsync(true);
			}
		}
	};
</script>

<style src="../Content/side-navigation-menu.css"></style>
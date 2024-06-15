<template>
    <div class="default-application-layout-component" v-if="menuItems">
        <header-toolbar class="layout-header"
                        :menu-toggle-enabled="menuItems.length > 0"
                        :toggle-menu-func="toggleMenu"
                        :title="title"
                        :is-x-small="isXSmall"
                        :is-small="isSmall"
                        :is-medium="isMedium"
                        :is-large="isLarge" 
                        />
        <dx-drawer class="layout-body"
                   position="before"
                   template="menuTemplate"
                   v-model:opened="menuOpened"
                   :opened-state-mode="drawerOptions.menuMode"
                   :reveal-mode="drawerOptions.menuRevealMode"
                   :min-size="drawerOptions.minMenuSize"
                   :shading="drawerOptions.shaderEnabled"
                   :close-on-outside-click="drawerOptions.closeOnOutsideClick">

            <dx-scroll-view class="layout-content-container with-footer">
                <template #default>
                    <slot name="content-header" />
                    <div class="progress-bar-break"></div>
                    <div class="content-footer-container">
                        <div class="content">
                            <slot name="default"></slot>
                        </div>
                        <slot name="footer" />
                    </div>
                </template>
            </dx-scroll-view>
            <loading-panel :container="loadingPanelContainer">
            </loading-panel>

            <!-- eslint-disable vue/no-unused-vars -->
            <template #menuTemplate="_">
                <side-nav-menu-component ref="navigationMenu"
                                         v-if="menuItems.length > 0"
                                         :compact-mode="!menuOpened"
                                         :items="menuItems"
                                         @click="handleSideBarClick"
                                         v-mounted="onNavigationMenuMountedAsync"
                                         v-unmounted="onNavigationMenuUnmountedAsync" />
            </template>
            <!-- eslint-enable -->
        </dx-drawer>
    </div>
</template>

<script>
    import DxDrawer from "devextreme-vue/drawer";
    import DxScrollView from "devextreme-vue/scroll-view";
    import NavigationMenuService from "@/Fwamework/NavigationMenu/Services/navigation-menu-service";
    import HeaderToolbar from "@UILibrary/Modules/HeaderToolbar/Components/HeaderToolbarComponent.vue";
    import SideNavMenuComponent from "@/Fwamework/NavigationMenu/Components/SideNavigationMenuComponent.vue";
    import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
    import { defineAsyncComponent } from "vue";
    import { useScreenSizeInfo } from "@/Fwamework/Utils/Services/screen-size-info";
    const LoadingPanel = defineAsyncComponent(() => import('@UILibrary/Fwamework/LoadingPanel/Components/LoadingPanelComponent.vue'));

    export default {
        props: {
            title: String
        },
        setup() {
            const { isXSmall, isSmall, isMedium, isLarge } = useScreenSizeInfo();
            return { isXSmall, isSmall, isMedium, isLarge };
        },
        data() {
            return {
                loadingPanelContainer: "window",
                menuOpened: this.isLarge,
                menuTemporaryOpened: false,
                menuItems: null
            };
        },
        created: showLoadingPanel(async function () {

            this.menuItems = await NavigationMenuService.getMenuItemsAsync();
            NavigationMenuService.onMenuItemsUpdated(this.onMenuItemsUpdated);
            //NOTE: Change the loading panel container to cover only the page zone
            this.loadingPanelContainer = ".layout-content-container .dx-scrollable-wrapper";
        }),
        methods: {
            async refreshSideMenus() {
                await NavigationMenuService.reloadMenuItemsAsync();
                this.menuItems = await NavigationMenuService.getMenuItemsAsync();
            },
            toggleMenu(e) {
				const pointerEvent = e.event ?? e;
                pointerEvent.stopPropagation();
                if (this.menuOpened) {
                    this.menuTemporaryOpened = false;
                }
                this.menuOpened = !this.menuOpened;
            },
            handleSideBarClick(e) {
                if (e.target) {
                    if (e.target === this.$refs.navigationMenu.$el) {
                        if (this.menuOpened === false) this.menuTemporaryOpened = true;
                        this.menuOpened = true;
                    }

                    e.stopPropagation();
                }
            },
            onMenuItemsUpdated(menuItems) {
                this.menuItems = menuItems;
            },
            async onNavigationMenuMountedAsync() {
                await NavigationMenuService.mountedEvent.emitAsync({ component: this.$refs.navigationMenu });
            },
            async onNavigationMenuUnmountedAsync() {
                await NavigationMenuService.unmountedEvent.emitAsync(null);
            }
        },
        computed: {
            drawerOptions() {
                const shaderEnabled = !this.isLarge;
                return {
                    menuMode: this.isLarge ? "shrink" : "overlap",
                    menuRevealMode: this.isXSmall ? "slide" : "expand",
                    minMenuSize: this.isXSmall ? 0 : 60,
                    menuOpened: this.isLarge,
                    closeOnOutsideClick: shaderEnabled,
                    shaderEnabled
                };
            },
            headerMenuTogglerEnabled() {
                return this.isXSmall;
            }
        },
        watch: {
            isLarge() {
                if (!this.menuTemporaryOpened) {
                    this.menuOpened = this.isLarge;
                }
            },
            $route() {
                if (this.menuTemporaryOpened || !this.isLarge) {
                    this.menuOpened = false;
                    this.menuTemporaryOpened = false;
                }
            }
        },
        components: {
            DxDrawer,
            DxScrollView,
            HeaderToolbar,
            SideNavMenuComponent,
            LoadingPanel
        }
    };
</script>

<style src="../Content/default-application-layout.css"></style>
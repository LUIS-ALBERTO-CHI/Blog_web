<template>
	<div :class="{ box: true, 'box-with-border': showBorder }">

		<div v-if="menuItems || title" class="box-header">
			<div class="box-action-menu" v-if="menuItems">
				<action-menu v-bind="menuOptions" :items="menuItems"></action-menu>
			</div>
			<div class="box-title" v-if="title">
				<slot name="box-title">
					{{title}}
				</slot>
			</div>
			<div class="box-clear"></div>
		</div>
		<div v-else-if="menuItems" class="box-header-no-title">
			<div class="box-action-menu" v-if="menuItems">
				<action-menu :items="menuItems" v-bind="menuOptions"></action-menu>
			</div>
			<div class="box-clear"></div>
		</div>
		<div class="box-content">
			<slot>
			</slot>
		</div>
		<div class="box-footer" v-if="timeline && timeline.length">
			<div class="block">
				<timeline :items="timeline" />
			</div>
		</div>
	</div>
</template>

<script>
	import Timeline from "@/Fwamework/Users/Components/TimelineComponent.vue";
	import ActionMenu from "@UILibrary/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	export default {
		components: {
			Timeline,
			ActionMenu
		},
		props: {
			title: String,
			menuItems: { type: Array, required: false },
			menuOptions: { type: Object, required: false },
			timeline: { type: Array, required: false },
			showBorder: { type: Boolean, required: false, default: true }
		}
	}
</script>
<style type="text/css" src="./Content/box.css" />

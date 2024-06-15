<template>
	<page-container type="summary">
		<box title="Sample with weight on columns">
			<p>1 + 1 + 2, total weight is 4. On a full screen you should have the following widths: 25% 25% 50%.</p>
			<div class="sample-layout-container">
				<layout :addPadding="false">
					<column>
						<div class="sample-column-content">
							<p>No weight means a weight = 1</p>
							<child-component></child-component>
						</div>
					</column>
					<column :weight="1">
						<div class="sample-column-content">
							<p>Forced weight to 1</p>
						</div>
					</column>
					<column :weight="2">
						<div class="sample-column-content">
							<p>Forced weight to 2</p>
							<child-component></child-component>
						</div>
					</column>
				</layout>
			</div>
		</box>
		<box title="Same sample as previous, but with padding">
			<p>1 + 1 + 2, total weight is 4. On a full screen you should have the following widths: 25% 25% 50%.</p>
			<div class="sample-layout-container">
				<layout>
					<column>
						<div class="sample-column-content">
							<p>No weight means a weight = 1</p>
							<child-component></child-component>
						</div>
					</column>
					<column :weight="1">
						<div class="sample-column-content">
							<p>Forced weight to 1</p>
						</div>
					</column>
					<column :weight="2">
						<div class="sample-column-content">
							<p>Forced weight to 2</p>
							<child-component></child-component>
						</div>
					</column>
				</layout>
			</div>
		</box>
		<box title="Sample with a forced total weight (4)">
			<p>Forced total weight is 4. On a full screen you should have the following width: 25%.</p>
			<div class="sample-layout-container">
				<layout :forced-total-weight="4" :addPadding="false">
					<column>
						<div class="sample-column-content">
							<p>No weight means a weight = 1</p>
						</div>
					</column>
				</layout>
			</div>
		</box>
		<layout>
			<column>
				<box title="Box inside a layout, first">
					<p>Do not set addPadding property of layout (default value: <strong>true</strong>).</p>
				</box>
			</column>
			<column :weight="2">
				<box title="Box inside a layout, second">
					<p>Do not set addPadding property of layout (default value: <strong>true</strong>).</p>
				</box>
			</column>
		</layout>
		<box title="A box after a layout">
			<p>Auto margin test on the top of the box.</p>
		</box>
		<layout :forced-total-weight="2">
			<column>
				<box title="Layout after layout A">
					<p>Test of the top margin when a layout is after a layout.</p>
				</box>
			</column>
		</layout>
		<layout>
			<column>
				<box title="Layout after layout B1">
					<p>Test of the top margin when a layout is after a layout.</p>
				</box>
			</column>
			<column>
				<box title="Layout after layout B2">
					<p>Test of the top margin when a layout is after a layout.</p>
				</box>
			</column>
		</layout>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import Layout from '@/Modules/Layouts/Components/LayoutComponent.vue';
	import Column from '@/Modules/Layouts/Components/LayoutColumnComponent.vue';
	import ChildComponent from '@/Samples/Layout/Components/SampleChildComponent.vue';
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			Box,
			Layout,
			ChildComponent,
			Column,
			PageContainer
		},
		created: showLoadingPanel(async function () {

		}),
	}
</script>
<style type="text/css">

	.sample-layout-container {
		border: 5px solid skyblue;
	}

	.sample-column-content {
		border: 5px solid orange;
	}
</style>
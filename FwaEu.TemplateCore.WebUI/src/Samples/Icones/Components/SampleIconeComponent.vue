<template>
	<page-container type="summary">
		<box title="Icones Font Awesome Side Navigation Menu">
			<div style="display: flex;">
				<div style="width: 240px;">
					<side-nav-menu-component :items="sideNavMenuItems">
					</side-nav-menu-component>
				</div>
				<ul>
					<li>Icône parent type Duotone (prefixe "fad") + attribuer une couleur</li>
					<li>Icône enfant type Light (prefixe "fal")</li>
				</ul>
			</div>
		</box>

		<box title="Icones Picker">
			<icon-picker v-model="iconClass"></icon-picker>
			{{ iconClass }}
		</box>
		<box title="Icones Font Awesome free">
			<ul>
				<li><i class="fab fa-angellist fa-2x"></i></li>
				<li><i class="fab fa-amazon fa-2x"></i></li>
				<li><i class="fab fa-acquisitions-incorporated fa-2x"></i></li>
				<li><i class="fas fa-backspace fa-2x"></i></li>
				<li><i class="fas fa-battery-half fa-2x"></i></li>
				<li><i class="fas fa-hamburger fa-2x"></i></li>
				<li><i class="fas fa-horse fa-2x"></i></li>
			</ul>
		</box>
		<box title="Icones Font Awesome Pro">
			<ul>
				<li><i class="far fa-flag fa-2x" style="color:red"></i></li>
				<li><i class="fas fa-abacus fa-2x" style="color:blue"></i></li>
				<li><i class="far fa-alicorn fa-2x" style="color:pink"></i></li>
				<li><i class="fal fa-alien-monster fa-2x" style="color:grey"></i></li>
				<li><i class="fal fa-abacus fa-2x" style="color:orange"></i></li>
				<li><i class="fal fa-stream fa-2x" style="color:purple"></i></li>
			</ul>
		</box>
		<box title="Icones Font Awesome Pro Duotone">
			<ul>
				<li><i class="fad fa-flux-capacitor fa-2x" style="--fa-primary-color: gold; --fa-secondary-color: red"></i></li>
				<li><i class="fad fa-meteor fa-2x" style="--fa-primary-color:orange; --fa-secondary-color:blue"></i></li>
				<li><i class="fad fa-portal-exit fa-2x" style="color:purple"></i></li>
				<li><i class="fad fa-watch-calculator fa-2x" style="--fa-primary-color:orange; --fa-secondary-color:purple"></i></li>
				<li><i class="fad fa-starfighter fa-10x"></i></li>

			</ul>
		</box>
	</page-container>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import IconPicker from "@UILibrary/Modules/FontAwesome/Components/IconPickerComponent.vue";
	import SideNavMenuComponent from "@/Fwamework/NavigationMenu/Components/SideNavigationMenuComponent.vue";

	export default {
		components: {
			Box,
			PageContainer,
			IconPicker,
			SideNavMenuComponent
		},
		data() {
			return {
				iconClass: "fas fa-file-chart-line",
				sideNavMenuItems: [
					{
						"text": "Styles",
						"icon": "fad fa-palette",
						"color": "#0a7dd8",
						"expanded": true,
						"items": [
							{
								"text": "Layout",
								"path": "/LayoutSample",
								"icon": "fal fa-cube"
							},
							{
								"text": "Box",
								"path": "/SampleBox",
								"icon": "fal fa-box"
							},
							{
								"text": "Icones",
								"path": "/SampleIcone",
								"icon": "fal fa-icons"
							}
						]
					}
				]
			}
		}

	}
</script>
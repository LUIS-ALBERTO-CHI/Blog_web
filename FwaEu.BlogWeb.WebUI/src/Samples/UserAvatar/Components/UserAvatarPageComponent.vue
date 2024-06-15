<template>
	<page-container type="form">
		<div class="avatar-sample">
			<h1>Color generator</h1>
			<box>
				<div>
					Context:
					<dx-text-box :value="colorGeneratorContext" value-change-event="keyup"
								 @value-changed="onColorGeneratorContextChanged" />
				</div>
				<div class="block sample-generated-colors">
					<span :key="color.id" v-for="color in generatedColors" class="sample-color"
						  :title="color.hsl"
						  :style="'background-color:' + color.hsl + ';'">{{color.id}}</span>
				</div>
			</box>
			<h1>Editable sample</h1>
			<box>
				<layout :addPadding="false">
					<column>
						<div><user-avatar :key="renderKey" :user="editable" size="small" /> To use inside a combo</div>
						<div class="block"><user-avatar :key="renderKey" :user="editable" size="medium" /> {{editable.fullName}}</div>
						<div class="block"><user-avatar :key="renderKey" :user="editable" size="large" /></div>
						<div class="block"><user-avatar :key="renderKey" :user="editable" size="x-large" /></div>
					</column>
					<column :weight="2">
						<dx-form :form-data="editable" :colCount="2" @fieldDataChanged="onEditableFieldChanged">
							<dx-item data-field="id" :colSpan="2">
								<dx-required-rule />
							</dx-item>
							<dx-item data-field="firstName" />
							<dx-item data-field="lastName" />
							<dx-item data-field="fullName">
								<dx-required-rule />
							</dx-item>
							<dx-item data-field="avatarUrl"></dx-item>
						</dx-form>
					</column>
				</layout>
			</box>

			<h1>Samples</h1>
			<box :key="index" v-for="(sample, index) in samples" :title="sample.description">
				<layout :addPadding="false">
					<column>
						<div><user-avatar :user="sample.user" size="small" /> To use inside a combo</div>
						<div class="block"><user-avatar :user="sample.user" size="medium" /> {{sample.user.fullName}}</div>
						<div class="block"><user-avatar :user="sample.user" size="large" /></div>
						<div class="block"><user-avatar :user="sample.user" size="x-large" /></div>
					</column>
					<column :weight="2">
						<code>{{sample.user}}</code>
					</column>
				</layout>
			</box>
		</div>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import Layout from '@/Modules/Layouts/Components/LayoutComponent.vue';
	import Column from '@/Modules/Layouts/Components/LayoutColumnComponent.vue';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import UserAvatar from '@/Fwamework/Users/Components/UserAvatarComponent.vue'
	import { DxForm, DxItem, DxRequiredRule } from "devextreme-vue/form";
	import { DxTextBox } from 'devextreme-vue/text-box';
	import ColorGeneratorService from '@/Fwamework/Utils/Services/color-generator-service';

	export default {
		components: {
			Box,
			PageContainer,
			Layout,
			Column,
			UserAvatar,
			DxForm,
			DxItem,
			DxRequiredRule,
			DxTextBox
		},
		methods:
		{
			onEditableFieldChanged(e)
			{
				this.renderKey += 1;
			},
			onColorGeneratorContextChanged(e)
			{
				this.generatedColors = [...Array(500).keys()].map(function (k)
				{
					return {
						id: k,
						hsl: ColorGeneratorService.getColor(k, e.value)
					};
				});
			}
		},
		data()
		{
			return {
				colorGeneratorContext: 'user-avatar',
				generatedColors: null,
				renderKey: 0,
				editable: {
					"id": 1,
					"firstName": "Dimitri",
					"lastName": "Ashikhmin",
					"fullName": "Dimitri Ashikhmin",
					"avatarUrl": null
				},
				samples: [
					{
						"description": "Should display the avatar from url",
						"user":
						{
							"id": 1,
							"firstName": "Dimitri",
							"lastName": "Ashikhmin",
							"fullName": "Dimitri Ashikhmin",
							"avatarUrl": "https://secure.gravatar.com/avatar/af23151e5e4faa2485f40f2f30d54b1a?d=identicon&r=G&s=100"
						}
					}
					,
					{
						"description": "Should display DA",
						"user": {
							"id": 1,
							"firstName": "Dimitri",
							"lastName": "Ashikhmin",
							"fullName": "Dimitri Ashikhmin"
						}
					}
					,
					{
						"description": "WW is mostly the biggest char",
						"user": {
							"id": 2,
							"firstName": "Wilfried",
							"lastName": "de Wolfenstein",
							"fullName": "Wilfried de Wolfenstein"
						}
					}
					,
					{
						"description": "II is mostly the smallest char",
						"user": {
							"id": 3,
							"firstName": "Ingrid",
							"lastName": "In",
							"fullName": "Ingrid In"
						}
					}
					,
					{
						"description": "Should display RR",
						"user": {
							"id": 4,
							"firstName": "Romain",
							"lastName": "de la Roumanie",
							"fullName": "Romain de la Roumanie"
						}
					}
					,
					{
						"description": "Should display DD",
						"user": {
							"id": 5,
							"firstName": "de",
							"lastName": "de",
							"fullName": "De De"
						}
					}
					,
					{
						"description": "Should display JA",
						"user": {
							"id": 6,
							"firstName": "Jean-Pierre",
							"lastName": "d'Antoine de la Ferme aux Canards",
							"fullName": "Jean-Pierre d'Antoine de la Ferme aux Canards"
						}
					}
					,
					{
						"description": "Should display ??",
						"user": {
							"id": 7,
							"firstName": "'",
							"lastName": " ",
							"fullName": "'"
						}
					}
				]
			};
		},
		created: showLoadingPanel(async function ()
		{
			this.onColorGeneratorContextChanged({ value: this.colorGeneratorContext });
		})
	}
</script>
<style type="text/css" scoped>
	.avatar-sample code
	{
		background-color: #EAEAEA;
		padding: 20px;
		display: block;
		white-space: pre-wrap;
		overflow-x: auto;
	}

	.sample-generated-colors
	{
		line-height: 20px;
		text-align: center;
	}

	.sample-color
	{
		display: inline-block;
		width: 50px;
		height: 20px;
		cursor: help;
		margin: 0px 4px 4px 0px;
	}

		.sample-color:hover
		{
			outline: 2px solid #000000;
		}
</style>

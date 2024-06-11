<template>
	<component v-if="model" :is="model.path ? 'router-link' : 'div'" :to="model.path">
		<box class="user-task-item" :style="taskItemStyle">
			<div :class="{'user-task-item-date': true, 'withtout-date': !model.date}" :style="taskItemDateStyle">
				<slot name="date">
					<span v-if="model.date" v-text="sinceDateLabel"></span>
					<date-literal :date="model.date" :null-text="nullDateText"></date-literal>
				</slot>
			</div>
			<div class="user-task-item-content">
				<slot>
					{{model.value}}
				</slot>
			</div>
			<div class="user-task-item-text">
				{{model.title}}
			</div>
		</box>
	</component>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import DateLiteral from "@UILibrary/Fwamework/Utils/Components/DateLiteralComponent.vue";
	import colorGeneratorService from "@/Fwamework/Utils/Services/color-generator-service";

	const userTaskBackgroundSaturation = 35;
	const userTaskBackgroundLightness = 50;
	export default {
		components: {
			Box,
			DateLiteral
		},
		props: {
			modelValue: {
				type: Object,
				required: true
			},
			displayZoneName: {
				type: String,
				required: true
			}
		},
		data() {
			const color = colorGeneratorService.getColor(this.modelValue.name, "user-tasks", userTaskBackgroundSaturation, userTaskBackgroundLightness);
			return {
				sinceDateLabel: this.$t('sinceDateLabel'),
				nullDateText: ' ',
				userTask: Object.assign(this.modelValue),
				model: null,
				taskItemStyle: {
					"color": color
				},
				taskItemDateStyle: {
					"background-color": color
				}
			};
		},
		async created() {
			this.model = await this.userTask.getValueAsync(this.displayZoneName);
			if (this.model.sinceDateLabel)
				this.sinceDateLabel = this.model.sinceDateLabel;
			if (this.model.nullDateText)
				this.nullDateText = this.model.nullDateText;
		}
	}
</script>
<style>
	.user-task-item .user-task-item-date {
		color: white;
		border-top-left-radius: 10px;
		border-top-right-radius: 10px;
	}

	.user-task-item .user-task-item-date {
		min-height: 20px;
	}

	.user-task-item .user-task-item-date,
	.user-task-item .user-task-item-content,
	.user-task-item .user-task-item-text {
		text-align: center;
	}

	.user-task-item.box .box-header + .box-content {
		margin-top: 0;
	}

	.user-task-item.box {
		padding: 0px;
		padding-bottom: 10px;
	}

	.user-task-item .user-task-item-content {
		font-size: 2rem;
	}

	.user-task-item-container a {
		text-decoration: none;
	}
</style>
<i18n>
	{
	"fr":{
	"sinceDateLabel": "Depuis le"
	},
	"en":{
	"sinceDateLabel": "Since"
	}
	}
</i18n>
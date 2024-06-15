<template>	
	<div v-if="displayText !== ''" :class="[ 'item-counter']"
		 :title="title"
		 v-bind="dataParams">
		{{displayText}}
	</div>
</template>

<script>
	export default {
		props: {
			title: {
				type: String
			},
			value: {
				type: Number,
				default: 0,
			},
			params: {
				type: Object,
				default: {}
			},
			minDisplay: {
				type: Number,
				default: 1
			},
			maxCount: {
				type: Number,
				default: 99
			}
		},
		computed: {
			dataParams() {
				let result = {};
				for (let [key, value] of Object.entries(this.params)) {
					result[`data-${key}`] = value;
				}
				return result;
			},
			displayText() {
				if (this.value < this.minDisplay)
					return '';
				return this.value > this.maxCount ? `${this.maxCount}+` : this.value;
			}
		}
	}
</script>

<style src="./Content/notification-counter-badge.css">

</style>

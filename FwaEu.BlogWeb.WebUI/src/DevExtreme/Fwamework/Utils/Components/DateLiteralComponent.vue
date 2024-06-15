<template>
	
	<span ref="dateLiteral" :class="cssClasses">
		{{ getDateText() }}
		<dx-tooltip v-if="date && useTooltip"
					v-model:target="tooltipTarget">
			{{ getTooltipText() }}
		</dx-tooltip>
	</span>
	
	
</template>

<script>
	import { DxTooltip } from "devextreme-vue/tooltip";


	export default {
		components: {
			DxTooltip
		},
		props: {
			date: Date,
			displayFormat: {
				type: String,
				default: "short"
			},
			nullText: String,
			useTooltip: {
				type: Boolean,
				default: true
			}
		},
		data() {
			return {
				tooltipTarget: null
			};
		},
		mounted() {
			this.tooltipTarget = this.$refs.dateLiteral;
		},
		methods: {
			getTooltipText() {
				return this.useTooltip && this.date ? this.$d(this.date, 'long') : null;
			},
			getDateText() {
				return this.date ? this.$d(this.date, this.displayFormat) : this.getNullText();
			},
			getNullText() {
				return this.nullText || this.$t("defaultNullDateText");
			}
		},

		computed: {
			cssClasses() {
				let classes = ['date-literal'];
				if (this.useTooltip)
					classes.push('with-title');
				if (!this.date)
					classes.push('with-null');
				return classes;
			}
		}
	}
</script>
<i18n>
	{
	"en": {
	"defaultNullDateText": "Never"
	},
	"fr": {
	"defaultNullDateText": "Jamais"
	}
	}
</i18n>
<style scoped>
	.date-literal.with-title:hover {
		cursor: help;
	}

	.date-literal.with-null {
		font-style: italic;
		color: lightgray;
	}
</style>

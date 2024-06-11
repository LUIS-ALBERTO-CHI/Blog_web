<template>
	<span 
		:class="cssClasses" 
		v-tooltip="getTooltipText()">
		{{ getDateText() }}
	</span>
</template>

<script>
export default {
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


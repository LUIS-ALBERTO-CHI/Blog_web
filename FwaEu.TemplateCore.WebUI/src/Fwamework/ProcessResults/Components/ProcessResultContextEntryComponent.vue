<template>
	<div :class="'process-result-entry ' + (details ? 'with-details ' : '') + type">
		<div class="process-result-entry-content">
			{{content}}
		</div>
		<div v-if="details">
			<div :key="index" v-for="(detail, index) in details" class="process-result-entry-detail">
				<i class="fa-solid fa-caret-right"></i>
				{{detail}}
			</div>
		</div>
	</div>
</template>
<script>
	export default {
		props: {
			entry: {
				type: Object,
				default: () => { }
			}
		},
		data()
		{
			return {
				type: this.entry.type,
				content: this.content,
				details: null
			}
		},
		mounted()
		{
			if (this.entry)
			{
				this.type = this.entry.type;
				this.content = this.entry.content;
				if (this.entry.details)
				{
					this.details = this.entry.details;
				}
			}
		}
	}
</script>
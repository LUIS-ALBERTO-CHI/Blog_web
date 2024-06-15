<template>
	<div :class="getClasses">
		<slot>
		</slot>
	</div>
</template>
<script>
	import { computed, reactive } from 'vue';

	export default {
		setup() {
			const columns = reactive([]);
			return {
				columns,
				totalWeight: computed(() => columns.map(x => x.weight).reduce((a, b) => a + b, 0))
			};
		},
		props: {
			forcedTotalWeight: Number,
			addPadding: { type: Boolean, default: true }
		},
		provide() {
			return {
				currentLayoutState: computed(() => ({
					columns: this.columns,
					totalWeight: this.forcedTotalWeight ?? this.totalWeight
				}))
			};
		},
		computed: {
			getClasses: function () {
				return 'pure-g layout' + (this.$props.addPadding ? ' layout-with-padding' : '');
			}
		}
	}
</script>
<style type="text/css" src="./Content/layout.css"></style>
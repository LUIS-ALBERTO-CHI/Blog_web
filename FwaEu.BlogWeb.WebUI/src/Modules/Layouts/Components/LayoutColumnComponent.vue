<template>
	<div :class="cssClasses">
		<slot />
	</div>
</template>

<script>
	export default {
		props: {
			weight: { type: Number, default: 1 },
		},
		data() {
			return {
				layoutComponentType: 'column'
			}
		},
		inject: ['currentLayoutState'],
		created() {
			this.currentLayoutState.columns.push({ component: this, weight: this.weight });
		},
		computed: {
			cssClasses() {
				if (!this.currentLayoutState.totalWeight)
					return '';
				//NOTE: From https://stackoverflow.com/questions/4652468/is-there-a-javascript-function-that-reduces-a-fraction
				function reduce(numerator, denominator) {
					var gcd = function gcd(a, b) {
						return b ? gcd(b, a % b) : a;
					};
					gcd = gcd(numerator, denominator);
					return [numerator / gcd, denominator / gcd];
				}

				var x = this.weight;
				var y = this.currentLayoutState.totalWeight || 1;

				[x, y] = reduce(x, y);

				return `layout-column pure-u-1 pure-u-lg-${x.toString()}-${y.toString()}`;
			}
		}
	}

</script>
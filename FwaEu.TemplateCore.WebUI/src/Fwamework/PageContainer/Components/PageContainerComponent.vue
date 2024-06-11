<template>
	<div :class="pageContainerClasses">
		<div class="page-container-content">
			<slot />
		</div>
	</div>
</template>

<script>
	export default {
		props: {
			type: {
				type: String,
				default: 'normal',
				validator: function (value)
				{
					return ['list', 'form', 'summary'].indexOf(value) !== -1
				}
			},
			customClass: {
				type: String,
				default: null
			}
		},
		computed: {
			pageContainerClasses()
			{
				return `page-container ${this.type} ${this.customClass}`.trimRight();
			}
		}
	}
</script>
<style src="./Content/page-container.css" />
<style src="./Content/page-print.css" />

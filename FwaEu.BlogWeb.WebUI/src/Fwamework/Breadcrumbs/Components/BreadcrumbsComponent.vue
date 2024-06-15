<template>
	<div class="main-container" >
		<breadcrumbs-collapsed-reduce v-if="breadcrumbs?.length > 0" :crumbs="breadcrumbs"></breadcrumbs-collapsed-reduce>
	</div>
</template>

<script>
	import BreadcrumbService from '../Services/breadcrumbs-service'
	import ResolveContext from '../Services/resolve-context'
	import BreadcrumbsCollapsedReduce from '@/Fwamework/Breadcrumbs/Components/BreadcrumbsCollapsedReduceComponent.vue';

	export default {
		components: {
			BreadcrumbsCollapsedReduce
		},
		data()
		{
			return {
				resolvedNodes: [],
				onRouteProcessedListener: BreadcrumbService.onRouteProcessed(this.onRouteProcessed)
			}
		},
		watch: {
			async $route(to)
			{
				await this.resolveBreadcrumb(to);
			}
		},
		async mounted()
		{
			await this.resolveBreadcrumb(this.$route);
		},
		methods: {
			async resolveBreadcrumb(route)
			{
				const context = new ResolveContext(this.$router, this.$i18n);
				await BreadcrumbService.processRouteAsync(route, context);
				this.$emit('change', this.breadcrumbs);
			},
			onRouteProcessed(resolved) {
				this.resolvedNodes = resolved.breadcrumbNodes;
			}
		},
		computed: {
            breadcrumbs() {
                return this.resolvedNodes.slice(0).reverse();
            }
		},
		beforeUnmount() {
			this.onRouteProcessedListener();
		}
	}
</script>
<template>
	<img :src="companyLogo" :height="height" :width="width" />
</template>

<script>
	import CompanyLogo from '@/TemplateCore/Content/logo-company.png';

	export default {
		props: {
			width: {
				type: Number,
				default: null
			},
			height: {
				type: Number,
				default: 30
			}
		},
		data: function () {
			return {
				companyLogo: CompanyLogo
			}
		}
	}
</script>

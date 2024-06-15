export default {
	getCurrentZoneName(vueInstance) {
		if (vueInstance.$route && vueInstance.$route.meta && vueInstance.$route.meta.zoneName) {
			return vueInstance.$route.meta.zoneName;
		}
		return "default";
	}
}

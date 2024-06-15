import { onMounted, onUnmounted, ref } from "vue";
import { sizes, subscribe, unsubscribe } from "@UILibrary/Extensions/Content/utils/media-query";


export const useScreenSizeInfo = function () {

	const screenSizeInfo = {
		isXSmall: ref(false),
		isSmall: ref(false),
		isMedium: ref(false),
		isLarge: ref(false),
		cssClasses: ref([])
	};

	function updateScreenSizeInfo() {
		const screenSizes = sizes();
		screenSizeInfo.isXSmall.value = screenSizes["screen-x-small"];
		screenSizeInfo.isSmall.value = screenSizes["screen-small"];
		screenSizeInfo.isMedium.value = screenSizes["screen-medium"];
		screenSizeInfo.isLarge.value = screenSizes["screen-large"];
		screenSizeInfo.cssClasses.value = Object.keys(screenSizes).filter(cl => screenSizes[cl]);
	}

	updateScreenSizeInfo();

	onMounted(() => subscribe(updateScreenSizeInfo));
	onUnmounted(() => unsubscribe(updateScreenSizeInfo));


	return screenSizeInfo;
}
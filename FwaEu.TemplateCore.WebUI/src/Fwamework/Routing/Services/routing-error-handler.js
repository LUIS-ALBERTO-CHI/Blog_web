import { isNavigationFailure } from "vue-router";

export default {
    async onErrorAsync(error) {
        if (isNavigationFailure(error)) {
            error.isHandled = true;
        }
	}
};

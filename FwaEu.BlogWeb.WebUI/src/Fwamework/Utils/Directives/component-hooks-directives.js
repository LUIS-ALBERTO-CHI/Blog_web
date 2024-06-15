export const OnCreated = {
	created(el, binding, vnode) {
		binding.value();
	}
}

export const OnBeforeMount = {
	beforeMount(el, binding, vnode) {
		binding.value();
	}
}

export const OnMounted = {
	mounted(el, binding, vnode) {
		binding.value();
	}
}

export const OnBeforeUpdate = {
	beforeUpdate(el, binding, vnode) {
		binding.value();
	}
}

export const OnUpdated = {
	updated(el, binding, vnode) {
		binding.value();
	}
}

export const OnBeforeUnmount = {
	beforeUnmount(el, binding, vnode) {
		binding.value();
	}
}

export const OnUnmounted = {
	unmounted(el, binding, vnode) {
		binding.value();
	}
}
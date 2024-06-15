<template>
    <div class="breadcrumbs">
        <div class="crumb-container-collapsed crumb-container-hidden">
            <div v-for="(link, index) in props.crumbs" :key="getNodeKey(link)" :href="link.to"
                 class="crumb" @click="nodeClicked(link)" :ref="(el) => crumbRefFn(el, index)">{{link.text}}</div>
        </div>
        <div class="crumb-container-collapsed">
            <div class="dropdown" :style="{ display: hiddenCount > 0 ? 'flex' : 'none' }">
                <button @click="toggleCollapsed" class="dropdown-button"><i class="fa-solid fa-ellipsis dropdown-button-icon"></i></button>
                <div class="dropdown-content" v-if="showCollapsed">
                    <router-link v-for="(link, index) in crumbsCollapsed" :key="getNodeKey(link)" :to="link.to" @click="nodeClicked(link)">{{link.text}}</router-link>
                </div>
            </div>
            <span v-for="(link, index) in crumbsVisible" :key="getNodeKey(link)" class="crumb">
                <router-link v-if="crumbsVisible.length -1 != index" :to="link.to" @click="nodeClicked(link)" class="crumb-link">{{link.text}}</router-link>
                <span v-else class="crumb-no-link">{{link.text}}</span>
                <span v-if="crumbsVisible.length -1 != index" :key="index" class="crumb-separator">&nbsp;></span>
            </span>
        </div>
    </div>
</template>
<script setup>
    import { ref,  computed, onMounted, onBeforeUnmount } from "vue";
    import { useIntersectionList } from "../Services/intersection-observer-service";
    import BreadcrumbService from '../Services/breadcrumbs-service'

    const props = defineProps({ crumbs: { type: Array, required: true } });

    const rootRef = ref();
    const showCollapsed = ref(false);

    const { hiddenCount, crumbRefFn } = useIntersectionList({
        list: props.crumbs,
        root: rootRef,
    });

    onMounted(() => {
        document.addEventListener("click", closeDropdown);
    });

    onBeforeUnmount(() => {
        document.removeEventListener("click", closeDropdown);
    });

	const closeDropdown = (event) => {
		if (!event.target.closest(".dropdown")) {
			showCollapsed.value = false;
		}
    };

	const toggleCollapsed = () => {
		showCollapsed.value = !showCollapsed.value;
	}

    const getNodeKey = (node) => {
       return node.name + JSON.stringify(node.to);
    }

    const nodeClicked = (node) => {
        hiddenCount.value = 0;
        return BreadcrumbService.nodeClicked.emitAsync({ component: this, node });
    }

    const crumbsCollapsed = computed(() => {
        return props.crumbs.slice(0, hiddenCount.value);
    });

    const crumbsVisible = computed(() => {
        return props.crumbs.slice(hiddenCount.value, props.crumbs.length)
    });
</script>

<style src="./Content/breadcrumbs.css" />
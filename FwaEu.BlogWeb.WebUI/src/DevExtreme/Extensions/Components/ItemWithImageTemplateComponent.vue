<template>
    <div class="custom-item">
        <img v-show="itemData.imageSrc" :src="itemData.imageSrc">
        <div class="custom-item-name">
            {{ itemData.name }}
        </div>
    </div>
</template>
<script>
    export default {
        props: {
            itemData: {
                type: Object,
                default: () => { }
            }
        }
    };
</script>


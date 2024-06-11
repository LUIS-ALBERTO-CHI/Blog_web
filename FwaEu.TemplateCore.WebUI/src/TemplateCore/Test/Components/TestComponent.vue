<template>
    TypeScript
    {{ test }}
</template>
<script lang="ts">

    import { defineComponent } from 'vue';

    export default defineComponent({
        data(){
            return {
                test: "test" as String
            }
        },
        created(){
        }
    })
</script>
<style lang="scss">
</style>
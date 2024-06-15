<template>
    <div class="custom-item">
        <img v-show="imageSrc" :src="imageSrc">
        <dx-text-box :value="name"
                     :read-only="true"
                     class="custom-item-name" />
    </div>
</template>
<script>
    import { DxTextBox } from 'devextreme-vue/text-box';

    export default {
        components: {
            DxTextBox,
        },

        props: {
            fieldData: {
                type: Object,
                default: () => { }
            }
        },

        computed: {
			imageSrc: function () {
				return this.fieldData ? this.fieldData.imageSrc : ''
			},
            name: function () { return this.fieldData ? this.fieldData.name : '' }
        }
    };
</script>
<style>
    .custom-item {
        position: relative;
        align-items: center;
    }

    .custom-item > img {
        padding-left: 8px;
    }

    .custom-item .custom-item-name {
        display: inline-block;
        padding-left: 50px;
        text-indent: 0;
        width: 100%;
    }

    .custom-item > img {
        left: 1px;
        position: absolute;
        top: 50%;
        margin-top: -5px;
    }

    .custom-item .dx-texteditor-buttons-container {
        display: none;
    }

    .custom-item-name {
            padding-left: 58px;
        }
</style>


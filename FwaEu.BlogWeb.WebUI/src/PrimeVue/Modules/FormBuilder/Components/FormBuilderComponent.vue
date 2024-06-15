<template>
    <div class="form-container">
        <div class="grid">
            <slot name="header" class="col-12" />

            <slot name="default" :columns="columns" :items="formItems" />
            <div class="gridInput nested-grid">
                <div v-for="item in formItemsToRender" :class="item.cssClasses" :key="item.name + (item.visibleIndex ?? 0)">
                    <label v-if="item.label && item.label.labelMode != 'hidden'" class="col-12" :for="item.dataField">{{item.label?.text}} </label>
                    <div class="col-12">
                        <component :is="item.component"
                                   v-model="item.validationField.value"
                                   :name="item.dataField"
                                   :class="{ 'col-12': true, 'p-invalid' : item.validationField.errorMessage}"
                                   v-bind="item.editorOptions"
                                   v-created=" item.editorOptions?.onCreated ?  ()=> item.editorOptions.onCreated({item, component:$refs[item.dataField], value: item.validationField.value}) : ()  => {} ">
                        </component>
                        <InlineMessage v-if="item.showErrorMessage && item.validationField?.errorMessage" severity="error">
                            {{ item.validationField.errorMessage }}
                        </InlineMessage>
                    </div>
                </div>
            </div>
            <slot name="footer" class="col-12" />
        </div>
    </div>
</template>
<script>
    import { defineAsyncComponent, defineComponent, markRaw } from 'vue';
    import { resolveComponentName } from '@/PrimeVue/module';
    import { useForm, useField } from 'vee-validate';
    import { ref } from 'vue';
    import { buildValidations } from "@/Modules/VeeValidation/Services/vee-validation-service";
    import { useScreenSizeInfo } from "@/Fwamework/Utils/Services/screen-size-info";
    import FormItem from "@/PrimeVue/Modules/FormBuilder/Components/FormItemComponent.vue"
    import { computed } from 'vue'
    import InlineMessage from 'primevue/inlinemessage';

    const primeVueComponents = import.meta.glob([
        '/node_modules/primevue/*/*.vue', 
        '!**/Chart.vue',//Ignore because it requires chart.js dependency
        '!**/Editor.vue']);//Ignore because it requires quill dependency

    let componentsConfiguration = Object.keys(primeVueComponents).map((e) => {
        const pathParts = e.split('/');
        const componentName = pathParts[pathParts.length - 1].replace(".vue", "");
        return {
            path: e,
            componentName: componentName,
            component: markRaw(defineAsyncComponent(primeVueComponents[e]))
        }
    });

    export default defineComponent({
        components: {
            FormItem,
            InlineMessage
        },
        props: {
            items: { type: Array },
            modelValue: Object,
            colCount: { type: Number, default: 1 }
        },
        setup(props, ctx) {
            const { isXSmall, isSmall, isMedium, isLarge } = useScreenSizeInfo();
            const getComponent = (item) => {
                return item.component ?? componentsConfiguration.find((e) => e.componentName == resolveComponentName(item.editorType))?.component;
            }

            const form = useForm({
                initialValues: props.modelValue
            });

            const formItems = ref([]);
            const formItemsToRender = computed(() => formItems.value.map(x => x).sort((a, b) => a.visibleIndex - b.visibleIndex));


            const mapFormItem = (item, initialLoad) => {
                const formItem = {
                    label: item.label,
                    dataField: item.dataField,
                    showErrorMessage: item.showErrorMessage ?? false,
                    name: item.name ?? item.dataField,
                    validationField: initialLoad ? useField(item.dataField, buildValidations(item)) : undefined,
                    component: getComponent(item),
                    editorOptions: item.editorOptions,
                    cssClasses: (item.cssClasses ?? []).concat(['col-' + columns.value, 'form-item', 'form-item-' + (item.name ?? item.dataField)])
                }
                if (item.label?.labelMode == 'floating') {
                    formItem.cssClasses.push('p-float-label');
                }
                return formItem;
            };

            const copyItem = (source, target) => {
                //NOTE: Try to set only required properties in order to prevent useless Vue.js changes detection triggering
                if (target.label != source.label) target.label = source.label;
                if (target.dataField != source.dataField) target.dataField = source.dataField;
                if (target.component != source.component) target.component = source.component;
                if (target.editorOptions != source.editorOptions) target.editorOptions = source.editorOptions;
                if (target.cssClasses != source.cssClasses) target.cssClasses = source.cssClasses;
            }

            const columnsFromProps = computed(() => {
                return Math.floor(12 / props.colCount);
            });

            const columns = computed(() => {
                return (isXSmall ? 12 : columnsFromProps.value);
            });
            const loadFormItemsFromSlot = (initialLoad) => {

                const defaultSlotContent = ctx.slots.default();
                const newFormItems = [];
                const updatedFormItems = [];
                if (defaultSlotContent?.length) {

                    for (const item of defaultSlotContent.filter(x => x.props)) {
                        const itemFromProps = { ...item.props };
                        const formItem = mapFormItem(itemFromProps, initialLoad);
                        const existingFormItem = formItems.value.find(x => x.name == formItem.name);
                        if (existingFormItem) {
                            copyItem(formItem, existingFormItem);
                            updatedFormItems.push(existingFormItem);
                        } else {
                            newFormItems.push(formItem);
                        }
                    }
                }

                for (const formItemToRemove of formItems.value.filter(x => updatedFormItems.every(ui => ui.name !== x.name))) {
                    const indexOfItemToRemove = formItems.value.indexOf(formItemToRemove);
                    formItems.value.splice(indexOfItemToRemove, 1);
                }
                for (const formItemToAdd of newFormItems) {
                    formItems.value.push(formItemToAdd);
                }
            };

            if (props.items?.length) {
                for (let item of props.items) {
                    formItems.value.push(mapFormItem(item, true));
                }
            } else {
                loadFormItemsFromSlot(true);
            }

            return { formItems, formItemsToRender, form, isXSmall, isSmall, isMedium, isLarge, columns, loadFormItemsFromSlot }
        },
        methods: {
            async validateForm() {
                const result = await this.form.validate();
                return result.valid;
            }
        },
        watch: {
            'form.values': {
                handler() {
                    this.$emit('update:modelValue', this.form.values);
                },
                deep: true,
            },
            'item.editorOptions': {
                handler() {
                    console.log(this.formItems)
                }
            }
        }
    })
</script>



<style lang="scss">

	.form-item {
		.p-inputtext {
			flex: 0 0 auto;
			padding: 0.5rem;
			width: 100%;
		}

		.p-component.p-inputwrapper {
			padding: 0px;
		}

		.p-inputswitch.p-component.col-12 {
			width: 3rem;
		}
	}

	.gridInput {
		width: 100%;
	}
</style>
<template>
    <PageContainer type="form">
        <box title="3 columns">
            <FormBuilder :items="dataFormItemsArray" v-model="personData" :col-count="3" ref="form3" />
            <Button @click="validateForm3()"> Validate</Button>
        </box>
        <box title="2 columns">
            <FormBuilder :items="dataFormItemsArray" v-model="personData" :col-count="2" ref="form2" />
            <Button @click="validateForm2()"> Validate</Button>
        </box>
        <box title="1 columns">
            <FormBuilder :items="dataFormItemsArray" v-model="personData" :col-count="1" ref="form1" />
            <Button @click="validateForm1()"> Validate</Button>
        </box>

        <box title="Validation with errors">
            <FormBuilder ref="formWithValidationErrors" :col-count="1" v-model="accountDetails">
                <FormItem dataField="identity"
                          :label="{ text: $t('mail') }"
                          :showErrorMessage="true"
                          :validationRules="[{type: 'required', errorMessage: 'toto'}, {type: 'email', errorMessage: 'toto'}]" />
                <FormItem dataField="password"
                          editorType="Password"
                          :label="{ text: $t('password') }"
                          :showErrorMessage="true"
                          :editorOptions="{ feedback: false }"
                          :validationRules="[{type: 'required'}, {type: 'min:6'}, {type: 'max:20'}]" />
                <FormItem dataField="firstName"
                          :label="{ text: $t('firstName') }"
                          :showErrorMessage="true"
                          :validationRules="[{type: 'required'}]" />
                <FormItem dataField="lastName"
                          :label="{ text: $t('lastName') }"
                          :showErrorMessage="true"
                          :validationRules="[{type: 'required'}]" />
            </FormBuilder>
            <Button label="Validate" @click="formWithValidationErrorsValidate" />
        </box>

        <box title="1 column declarative items">
            <FormBuilder v-model="personData">
                <FormItem dataField="firstName"
                          :label="{text: 'Custom first name label'}" />
                <FormItem dataField="lastName" />
            </FormBuilder>
        </box>
        <box title="3 columns declarative items">
            <FormBuilder v-model="personData" :col-count="3" ref="form4">
                <FormItem :dataField="dataFormItems.firstName.dataField"
                          :label="dataFormItems.firstName.label"
                          :visibleIndex="dataFormItems.firstName.visibleIndex"
                          :validationRules="dataFormItems.firstName.validationRules" />
                <FormItem :dataField="dataFormItems.lastName.dataField"
                          :label="dataFormItems.lastName.label"
                          :visibleIndex="dataFormItems.lastName.visibleIndex"
                          :validationRules="dataFormItems.lastName.editorOptions" />
                <FormItem :dataField="dataFormItems.select.dataField"
                          :label="dataFormItems.select.label"
                          :visibleIndex="dataFormItems.select.visibleIndex"
                          :editorType="dataFormItems.select.editorType"
                          :editorOptions="dataFormItems.select.editorOptions" />
                <FormItem :dataField="dataFormItems.select2.dataField"
                          :label="dataFormItems.select2.label"
                          :visibleIndex="dataFormItems.select2.visibleIndex"
                          :editorType="dataFormItems.select2.editorType"
                          :editorOptions="dataFormItems.select2.editorOptions" />
                <FormItem :dataField="dataFormItems.select3.dataField"
                          :label="dataFormItems.select3.label"
                          :visibleIndex="dataFormItems.select3.visibleIndex"
                          :editorType="dataFormItems.select3.editorType"
                          :editorOptions="dataFormItems.select3.editorOptions" />
                <FormItem :dataField="dataFormItems.number.dataField"
                          :label="dataFormItems.number.label"
                          :visibleIndex="dataFormItems.number.visibleIndex"
                          :editorType="dataFormItems.number.editorType"
                          :validationRules="dataFormItems.number.validationRules" />
                <FormItem :dataField="dataFormItems.email.dataField"
                          :label="dataFormItems.email.label"
                          :visibleIndex="dataFormItems.email.visibleIndex"
                          :validationRules="dataFormItems.email.validationRules" />
                <FormItem :dataField="dataFormItems.createdOn.dataField"
                          :label="dataFormItems.createdOn.label"
                          :visibleIndex="dataFormItems.createdOn.visibleIndex"
                          :editorType="dataFormItems.createdOn.editorType" />
            </FormBuilder>
            <Button @click="validateForm4()"> Validate</Button>
        </box>
    </PageContainer>
</template>

<script>

    import FormBuilder from "@/PrimeVue/Modules/FormBuilder/Components/FormBuilderComponent.vue";
    import FormItem from "@/PrimeVue/Modules/FormBuilder/Components/FormItemComponent.vue";
    import { dataFormItems, dataFormItemsArray, dataItems } from "@/Samples/FormBuilder/form-data-samples";
    import Button from "primevue/button";
    import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
    import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
    import { configure } from 'vee-validate';
    import { setLocale, localize } from '@vee-validate/i18n';
    import en from '@vee-validate/i18n/dist/locale/en.json';
    import fr from '@vee-validate/i18n/dist/locale/fr.json';

    export default {
        components: {
            FormBuilder,
            FormItem,
            Button,
            Box,
            PageContainer
        },
        created() {
            configure({
                generateMessage: localize({ en, fr })
            });
            setLocale('en');
        },
        data() {
            return {
                personData: dataItems,
                dataFormItems: dataFormItems,
                dataFormItemsArray: dataFormItemsArray,
                accountDetails: {
                    identity: null,
                    password: null,
                    firstName: null,
                    lastName: null
                }
            }
        },
        methods: {
            async formWithValidationErrorsValidate() {
                const result = await this.$refs.formWithValidationErrors.validateForm();
            },
            async validateForm1() {
                const result = await this.$refs.form1.validateForm();
            },
            async validateForm2() {
                const result = await this.$refs.form2.validateForm();
            },
            async validateForm3() {
                const result = await this.$refs.form3.validateForm();
            },
            async validateForm4() {
                const result = await this.$refs.form4.validateForm();
            }
        },
        watch: {
            personData: {
                handler() {
                    console.log(this.personData);
                },
                deep: true,
            },
        }
    }
</script>
<template>
    <FormBuilder ref="formLogin" :col-count="1" v-model="credentials">
        <FormItem dataField="identity"
                  :editorOptions="{placeholder:  $t('login')}"
                  :validationRules="[{type : 'required'}]" />
        <FormItem dataField="password"
                  editorType="Password"
                  :editorOptions="{placeholder: $t('password'), feedback: false }"
                  :validationRules="[{type : 'required'}]" />
    </FormBuilder>
    <div class="login-button">
        <Button ref="loginButton"
                :label="$t('button')"
                @click="onLoginClickAsync" />
    </div>
</template>

<script>
    import FormBuilder from "@/PrimeVue/Modules/FormBuilder/Components/FormBuilderComponent.vue";
    import FormItem from "@/PrimeVue/Modules/FormBuilder/Components/FormItemComponent.vue";
    import Button from 'primevue/button'
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
    import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
    import { AuthenticationHandlerKey } from '@/Modules/DefaultAuthentication/Services/default-authentication-handler';

    export default {
        components: {
            FormBuilder,
            FormItem,
            Button
        },
        data() {
            return {
                credentials: {
                    identity: "",
                    password: ""
                }
            };
        },
        async created() {
            await loadMessagesAsync(this, import.meta.glob('@/Modules/DefaultAuthentication/UserParts/Credentials/Components/Content/login-messages.*.json'));
        },
        methods: {
            onFormItemEntered() {
                this.$refs.loginButton.$el.click();
            },

            async onLoginClickAsync(e) {
                if (!await this.$refs.formLogin.validateForm()) {
                    return;
                }
                await AuthenticationService.loginAsync(AuthenticationHandlerKey, { identity: this.credentials.identity, password: this.credentials.password }).catch(error => {
                    if (error.response?.status === 401) {
                        NotificationService.showWarning(this.$t("loginErrorMessage"));
                    } else {
                        throw error;
                    }
                });
            }
        }
    };
</script>
<style type="text/css">
    .login-page .login-button {
        text-align: center;
    }
</style>
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import * as SentryVue from '@sentry/vue';
import { Integrations } from '@sentry/tracing';
import ErrorHandlerService from '@/Fwamework/Errors/Services/error-handler-service';
import CurrentUserService from '@/Fwamework/Users/Services/current-user-service';
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import Router from "@/Fwamework/Routing/Services/vue-router-service";

export class SentryModule extends AbstractModule {
	onInitAsync(vueApp) {
		if (Configuration.sentry.enabled) {
			SentryVue.init({
				app: vueApp,
				environment: import.meta.env.MODE,
				dsn: Configuration.sentry.dns,
				logErrors: true,
				integrations: [
					new Integrations.BrowserTracing({
						routingInstrumentation: SentryVue.vueRouterInstrumentation(Router),
						tracingOrigins: [Configuration.application.publicUrl]
					})
				]
			});

			ErrorHandlerService.onUnhandledError((error) => {
				SentryVue.captureException(error);
			});

			CurrentUserService.onChanged((event) => {
				SentryVue.addBreadcrumb({
					category: "auth",
					message: "Current user changed",
					data: event.currentUser,
					level: "info"
				});
			})
		}
	}
} 


export const Sentry = SentryVue;
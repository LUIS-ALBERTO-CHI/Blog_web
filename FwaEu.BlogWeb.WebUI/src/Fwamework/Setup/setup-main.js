import '@UILibrary/Extensions/Themes/generated/theme.base.css';

import App from '@/Fwamework/Setup/Components/SetupAppComponent.vue';
import { SetupModule } from '@/Fwamework/Setup/setup-module';
import { DialogModule } from '@/Modules/Dialog/dialog-module';
import { LoadingPanelModule } from '@/Fwamework/LoadingPanel/loading-panel-module';
import { UILibraryModule as DevExtremeModule } from '@/DevExtreme/module';
import { UILibraryModule } from '@UILibrary/module';
import { ErrorModule } from '@/Fwamework/Errors/error-module';
import Application from "@/Fwamework/Core/Services/application";
import { AuthenticationModule } from '@/Fwamework/Authentication/authentication-module';
import { UtilsModule } from '@/Fwamework/Utils/utils-module';

import { ImpersonateAuthenticationModule } from "@/Modules/ImpersonateAuthentication/impersonate-authentication-module";
import SetupImpersonateAuthenticationHandler from "@/Modules/ImpersonateAuthentication/Setup/setup-impersonate-authentication-handler";

import { RoutingModule } from '@/Fwamework/Routing/routing-module';
import AppRoutes from '@/Fwamework/Setup/setup-app-routes';

const application = new Application(App)
	.useModule(new SetupModule())
	.useModule(new DialogModule())
	.useModule(new ImpersonateAuthenticationModule())
	.useModule(new AuthenticationModule({
		authenticationHandlers: [
			SetupImpersonateAuthenticationHandler,
		]
	}) )
	.useModule(new DevExtremeModule())
	.useModule(new UILibraryModule())
	.useModule(new LoadingPanelModule())
	.useModule(new ErrorModule())
	.useModule(new RoutingModule({
		routerOptions: {
			routes: AppRoutes
		}
	}))
	.useModule(new UtilsModule());

application.mountAsync("#app");

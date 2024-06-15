import '@/Modules/NavigationIndicator/Content/progress-bar.scss';
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import NavigationIndicatorService from '@/Modules/NavigationIndicator/Services/navigation-indicator-service';

export class NavigationIndicatorModule extends AbstractModule {
	onInitAsync() {
		NavigationIndicatorService.configure();
	}
}
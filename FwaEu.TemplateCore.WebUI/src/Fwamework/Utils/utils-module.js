import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';

export class UtilsModule extends AbstractModule {
	async onInitAsync() {
		let textNormalizationService = (await import('@/Fwamework/Utils/Services/text-normalization-service')).default;
		textNormalizationService.addTextNormalization();
	}
}
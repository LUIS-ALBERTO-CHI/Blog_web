import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import { defineRule } from 'vee-validate';
import * as AllRules from '@vee-validate/rules';

export const rules = Object.keys(AllRules).map(rule => {
	 return defineRule(rule, AllRules[rule]);
});

export class VeeValidateModule extends AbstractModule {
	async onInitAsync(vueApp) {

	}
}

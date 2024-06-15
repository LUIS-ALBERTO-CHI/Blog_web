import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
import { markRaw } from "vue";

export default {
	data() {
		return {
			user: null,
			modelData: {},
			menuItems: null
		};
	},
	created: showLoadingPanel(async function () {
		await this.loadAsync();
	}),

	methods: {
		getUserAsync() {
			throw new Error("getUserAsync must be implemented");
		},
		getContextAsync() {
			throw Error("getContextAsync must be implemented");
		},
		bindModelByLocationAsync(model) {
			throw Error("bindModelByLocationAsync must be implemented");
		},
		getPartHandlers() {
			throw Error("bindModelByLocationAsync must be implemented");
		},
		getModelUserToSave() {
			throw Error("getModelUserToSave must be implemented");
		},
		getGeneralInformationComponent() {
			throw Error("getGeneralInformationComponent must be implemented");
		},
		saveUserAsync(userId, userToSave, partHandlers) {
			throw Error("saveUserAsync must be implemented");
		},
		async loadAsync() {
			const partHandlers = this.getPartHandlers();

			let user = await this.getUserAsync();

			const models = [];
			let context = await this.getContextAsync();

			let menuItems = [];
			const $this = this;
			const dispatchPromises = partHandlers.map(async function (partHandler) {
				
				let partModels = [];
				if (typeof partHandler.initializeAsync === 'function') {
					partModels = await partHandler.initializeAsync(user, context);
				}
				if (partHandler.createMenuItemsAsync) {
					menuItems = menuItems.concat(await partHandler.createMenuItemsAsync(user, context));
				}
				let refId = 0;

				for (let model of partModels) {
					const currentModel = {
						...model,
						handler: {
							...partHandler, component: undefined, createComponent: partHandler.component ? () => markRaw(partHandler.component) : undefined }
					};
					if (typeof currentModel.location === 'undefined') {
						currentModel.ref = currentModel.handler.partName + '_' + (++refId);
					}
					else {
						if (currentModel.location == "general-information") {
							user = Object.assign(user, currentModel.data);
							const modelFormItems = await currentModel.createFormItemsAsync(user, context);
							currentModel.formItems = modelFormItems;
						}
						else {
							await $this.bindModelByLocationAsync(currentModel);
						}
					}

					models.push(currentModel);
				}
			});

			await Promise.all(dispatchPromises);

			const visibleModels = this.getVisibleModels(user, models);
			await this.fetchModelsDataAsync(visibleModels);

			this.menuItems = menuItems;
			this.modelData = { models, visibleModels };//NOTE: We need to create modelData because: https://stackoverflow.com/questions/42629509/you-are-binding-v-model-directly-to-a-v-for-iteration-alias

			this.user = user;
		},
		saveAsync: showLoadingPanel(async function (e) {
			const $this = this;

			const userToSave = this.getModelUserToSave();
			let isValid = true;
			const generalInformation = this.getGeneralInformationComponent();
			if (generalInformation) {
				isValid = await generalInformation.validateAsync();
			}

			let context = await this.getContextAsync();
			Object.assign(context, {
				formModel: this.user
			});
			for (const model of this.modelData.models) {

				if (!this.modelMustBeSaved(this.user, model))
					continue;

				const handler = model.handler;
				Object.assign(context, {
					handlerModel: model
				});

				let component = Array.isArray(this.$refs[model.ref]) ? this.$refs[model.ref][0] : this.$refs[model.ref];
				
				//the component instance is not a required element of user parts, so it can be missing
				if (component
					&& typeof component.validateAsync !== 'undefined'
					&& !await component.validateAsync()) {

					isValid = false;
					return;
				}

				if (typeof handler.fillAsync === 'function') {
					await handler.fillAsync(userToSave, component, context);
				}
			}

			if (isValid) {
				const partHandlers = this.getPartHandlers();
				const userValid = await this.saveUserAsync(this.user.id, userToSave, partHandlers);

				if (!userValid) {
					return;
				}

				for (const handler of partHandlers) {
					if (handler.onUserSavedAsync) {
						await handler.onUserSavedAsync(userToSave, context);
					}
				}
			}

			if (!isValid) {
				this.$refs.accordion?.dataSource.forEach(item => {
					$this.$refs.accordion.instance.expandItem(item);
				});
			}
		}),
		async fetchModelsDataAsync(models) {
			await Promise.all(models.filter(m => typeof m.fetchDataAsync !== 'undefined' && !m.fetchedData)
				.map(async (m) => {
					let fetchedData = await m.fetchDataAsync();
					m.fetchedData = fetchedData;
				}));
		},
		modelMustBeSaved(user, model) {
			return true;
		},
		getVisibleModels(user, handlerModels) {
			return handlerModels;
		}
	},
	computed: {
		userPartsFormItems() {
			return this.modelData.visibleModels?.filter(x => x.formItems).flatMap(x => x.formItems);
		},
		visibleModelsWhichAreComponents() {
			return this.modelData.visibleModels.filter(m => typeof m.location === 'undefined');
		}
	}
}


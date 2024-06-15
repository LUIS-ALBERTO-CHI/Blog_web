import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
import UserService from '@/Fwamework/Users/Services/user-service';
import UserFormatterService from '@/Fwamework/Users/Services/user-formatter-service';
import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';

export default {
	data() {
		const $this = this;
		return {
			userLazy: new AsyncLazy(() => UserService.getUserByIdAsync($this.$route.params.id)),
			currentUser: null,
			timelineModel: null,
		};
	},
	created: showLoadingPanel(async function () {
		await this.loadAsync();
	}),
	methods: {
		getPartHandlers() {
			return UserPartsRegistry.getAll();
		},
		async getUserAsync() {
			return this.isNew
				? this.createNewUser()
				: await this.userLazy.getValueAsync();
		},
		async getContextAsync() {
			let currentUser = await CurrentUserService.getAsync();
			this.currentUser = currentUser;
			return {
				currentPage: this,
				isNew: this.isNew,
				currentUser: currentUser
			};
		},
		bindModelByLocationAsync(model) {
			switch (model.location) {
				case 'timeline':
					this.timelineModel = model.data
					break;

				default:
					throw 'Unknown location: ' + model.location;
			}
		},
		getModelUserToSave() {
			return {
				id: this.user.id,
				parts: {}
			};
		},
		getGeneralInformationComponent() {
			return this.$refs.generalInformation;
		},
		async saveUserAsync(userId, userToSave, partHandlers) {
			await UserService.saveAsync(userId, userToSave).then(() => {
				this.$router.push({
					name: 'Users'
				});
			}).catch(error => {
				if (error.response.data) {
					if (error.response.data.title === "") {
						NotificationService.showWarning(this.$t("emailInvalid"));
					}
					else {
						for (const handler of partHandlers) {
							if (handler.handleErrorAsync) {
								handler.handleErrorAsync(error.response.data);
							}
							if (error.response.data.status === 412 && error.response.data.title == handler.partName) {
								NotificationService.showWarning(this.$t(error.response.data.type));
							}
						}
					}
				}
			});
		},
		createNewUser() {
			return { id: 0, parts: {} };
		},
		async onNodeResolve(node) {
			if (this.$route.params.id) {
				const user = await this.userLazy.getValueAsync();
				node.text = UserFormatterService.getUserFullName(user);
			}
			return node;
		},
		async onIsAdminChangedAsync() {
			let visibleModels = this.getVisibleModels(this.user, this.modelData.models);
			await this.fetchModelsDataAsync(visibleModels);
			this.modelData.visibleModels = visibleModels;
		},
		modelMustBeSaved(user, model) {
			return this.isVisibleModel(user, model);
		},
		isVisibleModel(user, handlerModel) {
			return handlerModel.showForAdmin === undefined
				||
				(user.isAdmin && handlerModel.showForAdmin)
				||
				(!user.isAdmin && !handlerModel.showForAdmin);
		},
		getVisibleModels(user, handlerModels) {
			const $this = this;
			return handlerModels.filter(handlerModel => $this.isVisibleModel(user, handlerModel));
		}
	},
	computed: {
		isNew() {
			return !this.$route.params.id;
		}
	}
}
<template>
	<div :class="cssClasses"  ref="target">
		<div class="accordion-trigger"
			 @click="toggleAsync">

			<!-- This slot will handle the title/header of the accordion and is the part you click on -->
			<slot name="trigger">

				<toolbar class="accordion-item-title" :menu-items="item?.menu?.items" :menu-options="item?.menu?.options">
					<slot name="title" :item="item" />
				</toolbar>
				<i :class="{'accordion-item-trigger-icon': true,'fa-solid fa-chevron-down': !opened,'fa-solid fa-chevron-up': opened}"></i>
			</slot>
		</div>

		<transition name="accordion"
					@enter="start"
					@after-enter="end"
					@before-leave="start"
					@after-leave="end">

			<div class="accordion-content"
				 v-show="opened">
				<div>
					<!-- This slot will handle all the content that is passed to the accordion -->
					<slot name="content" :item="item">
						<p v-if="item?.data?.content">
							{{item?.data?.content}}
						</p>
					</slot>
				</div>
			</div>
		</transition>
	</div>
</template>


<script>
	import { inject, ref } from "vue";
	import { useFocusWithin, useVModel } from '@vueuse/core';
	import Toolbar from "@/Fwamework/Toolbar/Components/ToolbarComponent.vue";

	export default {
		components: {
			Toolbar
		},
		setup(props, { emit }) {
			const accordionData = inject(props.accordionId);
			const index = ref(accordionData.value.count.value++);
			const opened = useVModel(props, 'opened', emit, { defaultValue: true });

			if (opened.value && !accordionData.value.openedIndexes.value.includes(index.value)) {
				accordionData.value.openedIndexes.value.push(index.value);
			}

			const target = ref();
			const { focused } = useFocusWithin(target);

			return {
				accordionData,
				index,
				target,
				focused,
				opened
			};
		},
		props: {
			accordionId: {
				type: Symbol,
				required: true
			},
			opened: {
				type: Boolean,
				default: true
			},
			item: Object
		},
		methods: {
			async toggleAsync() {
				if (!this.opened) {
					await this.openAsync();
				}
				else if (this.opened) {
					await this.closeAsync();
				}
			},
			async openAsync() {
				await this.triggerEventAsync('opening', 'opened', () => {
					this.accordionData.openedIndexes.value.push(this.index);
					this.opened = true;
				});
			},
			async closeAsync() {
				await this.triggerEventAsync('closing', 'closed', () => {
					const itemIndex = this.accordionData.openedIndexes.value.indexOf(this.index.value);
					this.accordionData.openedIndexes.value.splice(itemIndex, 1);
					this.opened = false;
				});
			},
			async triggerEventAsync(eventName, eventSuccessName, callback) {
				const args = {
					index: this.index.value,
					item: this.item,
					openedIndexes: [...this.accordionData.openedIndexes.value],
					cancel: false
				};
				this.$emit(eventName, args);
				if (!await Promise.resolve(args.cancel)) {
					delete args.cancel;

					callback.bind(this)(args);
					this.$emit(eventSuccessName, args);
				}
			},
			start(el) {
				el.style.height = el.scrollHeight + "px";
			},
			end(el) {
				el.style.height = "";
			}
		},
		computed: {
			cssClasses() {
				return 'accordion-item'
					+ (this.opened ? ' accordion-item-opened' : '')
					+ (!this.opened ? ' accordion-item-closed' : '')
					+ (this.focused ? ' accordion-item-focused' : '')
					;
			}
		},
		watch: {
			async 'opened'() {
				if (this.opened)
					await this.openAsync();
				else 
					await this.closeAsync();
			}
		}
	};

</script>

<style lang="scss" scoped>
	.accordion-item {
		padding: 5px;
		cursor: pointer;
		border-color: transparent;
		border-style: solid;
		border-width: 1px;
		border-color: #ebebeb;
		position: relative;
	}
	.accordion-item-focused {
		border-radius: 4px;
		border-color: var(--secondary-text-color);
	}
	.accordion-item-closed {
		padding-top: 0;
		padding-bottom: 0;
	}

	.accordion-trigger {
		display: flex;
		justify-content: space-between;
	}
	.accordion-item-trigger-icon {
		display: flex;
		padding: 8px;
		align-items: center;
		font-size: 24px;
	}

	.accordion-item-title {
		width: 100%;
		font-size: 18px;
		color: var(--primary-text-color);
	}

	.accordion-enter-active,
	.accordion-leave-active {
		will-change: height, opacity;
		transition: height 0.3s ease, opacity 0.3s ease;
		overflow: hidden;
	}

	.accordion-enter,
	.accordion-leave-to {
		height: 0 !important;
		opacity: 0;
	}
</style>

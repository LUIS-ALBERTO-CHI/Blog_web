<template>
	<span :class="'avatar ' + size" :title="titleToDisplay">
		<img v-if="useUrl" :src="item.avatarUrl" alt="" />
		<span v-else class="avatar-initials" :style="'background-color:' + backgroundColor + ';'">{{initials}}</span>
	</span>
</template>
<script>
	import ColorGeneratorService from '@/Fwamework/Utils/Services/color-generator-service';

	export default {
		props: {
			item: {
				type: Object,
				required: true,
				validator: function (model) {
					return (model.fullName || model.name) && (model.avatarUrl || (model.id && model.initials));
				}
			},
			context: {
				type: String,
				default: 'avatar'
			},
			size: {
				type: String,
				default: "medium",
				validator: function (value) {
					return ["small", "medium", "large", "x-large"].includes(value);
				}
			}
		},
		data() {
			return {
				avatarItem: Object.assign({}, this.item)
			};
		},
		computed: {
			useUrl() {
				return typeof this.avatarItem.avatarUrl === 'string' && this.avatarItem.avatarUrl !== '';
			},
			initials() {
				return this.useUrl ? null : this.avatarItem.initials
			},
			backgroundColor() {
				return this.useUrl ? null : ColorGeneratorService.getColor(this.avatarItem.id, this.context);
			},
			titleToDisplay() {
				return this.item.fullName ?? this.item.name;
			}
		},
		watch: {
			item() {
				this.avatarItem = Object.assign({}, this.item);
			}
		}
	}
</script>
<style type="text/css" src="./Content/avatar.css"></style>
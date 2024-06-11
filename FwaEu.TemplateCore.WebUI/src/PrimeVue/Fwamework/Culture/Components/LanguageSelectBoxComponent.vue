<template>
		<Dropdown v-model="value" :options="languagesList" optionLabel="name" 
			showClear :class="['w-full', { 'p-invalid': errorMessage }]"> 
			<template #value="slotProps">
				<div v-if="slotProps.value" class="custom-dropdown-value">
					<img :alt="slotProps.value.imageSrc" :src="slotProps.value.imageSrc" />
					<span>{{ slotProps.value.name }}</span>
				</div>
			</template>
			<template #option="slotProps">
				<div class="custom-dropdown-option">
					<img :alt="slotProps.option.label" :src="slotProps.option.imageSrc" />
					<span>{{ slotProps.option.name }}</span>
				</div>
			</template>
		</Dropdown>
</template>

<script>

	import Dropdown from 'primevue/dropdown';
	import { useField } from 'vee-validate';

	export default {
		components: {
			Dropdown
		},
		props: {
			languageValue: {
				type: String
			},
			languages: {
				type: Array
			},
			isRequired: {
				type: Boolean
			}
		},
		created() {
			this.value = this.languagesList.find(x=>x.code === this.languageValue);
		},
		data() {
			const { value, errorMessage } = useField('language', (inputValue) =>
				!this.isRequired || (this.isRequired && !!inputValue)
			);
			return {
				value, errorMessage
			}
		},
		watch: {
			value() {
				(this.value) && this.$emit('update:languageValue', this.value.code);
			},
		},
		computed: {
			languagesList(){
				const $this = this;
				return this.languages.map(lang => {
					return {
						code: lang.code,
						name: $this.$t(lang.code),
						imageSrc: lang.imageSrc
					};
				});
			}
		}
	}

</script>
<style src="@/PrimeVue/Fwamework/Culture/Content/language-select-box.css"></style>
export function hasValidationRules(field) {
	return field?.validationRules?.length > 0;
}

export function buildValidations(field) {
	let validations = "";
	if (hasValidationRules(field)) {
		validations = field.validationRules?.map((e) => e = e.type).join('|');
	}
	return validations;
}
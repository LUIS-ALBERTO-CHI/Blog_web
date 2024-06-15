
//NOTE: Adapted from https://stackoverflow.com/questions/521295/seeding-the-random-number-generator-in-javascript
function generateNumber(id) {
	var x = Math.sin(id) * 10000;
	return x - Math.floor(x);
}

//NOTE: Taken from https://stackoverflow.com/questions/3895478/does-javascript-have-a-method-like-range-to-generate-a-range-within-the-supp
function hashCode(s) {
	return s.split('')
		.reduce((a, b) => { a = ((a << 5) - a) + b.charCodeAt(0); return a & a }, 0);
}

function intToHslColor(int, s, l) {
	var h = (int * 360) % 360;
	return 'hsl(' + h + ', ' + s + '%, ' + l + '%)';
}

export default {
	getColor(id, context, saturation = 30, lightness = 80) {
		return intToHslColor(generateNumber(hashCode(new String(id)) + hashCode(context)), saturation, lightness);
	}
}
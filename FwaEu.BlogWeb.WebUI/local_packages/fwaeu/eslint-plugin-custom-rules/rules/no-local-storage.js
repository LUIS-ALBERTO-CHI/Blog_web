const localStorageName = "localStorage";

module.exports = function (context) {
	return {
		"Identifier": function (node) {
			if (node.name === localStorageName) {
				context.report(node, "Don't use native localStorage directly, use 'LocalStorage' Store instead in order to prevent key conflicts with other applications.", { name: node.name });
			}
		}
	};
};

module.exports.schema = [];
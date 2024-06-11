import { Formatter } from "@/Modules/Formatter/Services/formatter-registry";

export class UserFormatter extends Formatter {
	key = 'user';
	async formatAsync(value) {
		return this.getUserFullName(value);
	}

	separators = [' ', '-', '\'', '‘'];
	particles = ["de", "du", "d", "l", "la", "le"];

	formattingInfo = {
		separators: this.separators,
		particles: this.particles
	};

	getUserFullName(user) {
		if (!user) {
			return null;
		}
		const fullName = this.getUserNameParts(user).map(up => this.formatName(up)).join(' ');
		return fullName.trim();
	}

	getUserNameParts(user) {
		return [(user.firstName ?? user.parts?.application?.firstName), (user.lastName ?? user.parts?.application?.lastName)];
	}

	generateInitials(user) {
		const info = this.formattingInfo;

		function getFirstPartUppercase(value) {
			var parts = value.split(new RegExp(info.separators.join("|"), "i"));

			for (const part of parts) {
				if (!part || info.particles.includes(part)) {
					continue;
				}

				return part[0].toUpperCase();
			}

			return ((parts[0] || "")[0] || "").toUpperCase() || "?"; //NOTE: For a name which is also a particle, or an invalid char
		}

		return this.getUserNameParts(user).map(up => getFirstPartUppercase(up)).join('');
	}

	formatName(value) {
		if (!value) {
			return '';
		}
		value = value.trim().toLowerCase();
		let hasSeparator = false;
		this.separators.forEach(separator => {
			if (value.includes(separator)) {
				const sb = [];
				let firstPass = true;
				const parts = value.split(separator);
				parts.forEach(part => {
					if (!firstPass) {
						sb.push(separator);
					}
					sb.push(this.particles.includes(part) ? part : this.firstLetterUpperCase(part));
					firstPass = false;
				});
				value = sb.join("");
				hasSeparator = true;
			}
		});
		if (!hasSeparator) {
			value = this.firstLetterUpperCase(value);
		}
		return value;
	}

	firstLetterUpperCase(part) {
		let hasSeparator = false;
		this.separators.forEach(separator => {
			if (part.includes(separator))
				hasSeparator = true;
		});
		return (hasSeparator ? part : part[0].toUpperCase() + part.substring(1));
	}
}

export default {
	userFormatter: new UserFormatter(),

	/** @param { UserFormatter } userFormatter */
	setUserFormatter(userFormatter) {
		this.userFormatter = userFormatter;
	},

	generateInitials(user) {
		return this.userFormatter.generateInitials(user);
	},

	getUserFullName(user) {
		return this.userFormatter.getUserFullName(user);
	}
}



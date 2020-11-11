/*
 * @bot-written
 * 
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 * 
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */

// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export function lowerCaseFirst(str: string) {
	return str[0].toLowerCase() + str.slice(1);
}

export function upperCaseFirst(str: string) {
	return str[0].toUpperCase() + str.slice(1);
}

export function camelCaseIntoWords(name: string) {
	return name.replace(/([A-Z])/g, ' $1')
		.replace(/^./, (str) => str.toUpperCase());
}

export function camelCase(str: string) {
	const words = str.split(" ");
	const allLowerCase = words.map(w => w.toLowerCase());
	return allLowerCase[0] + allLowerCase.slice(1).map(upperCaseFirst).join("");
}

export function lowerCaseNoSpaces(str: string) {
	return str.toLowerCase().replace(/ /g, "")
}

export function noSpaces(str: string) {
	return str.replace(/ /g, "")
}

// % protected region % [Add extra StringUtil functions here] off begin
// % protected region % [Add extra StringUtil functions here] end

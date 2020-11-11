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
import { Symbols } from 'Symbols';
import { initValidators, IModelAttributeValidationError, ErrorType } from '../Util';
import { Model } from 'Models/Model';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export default function validate() {
	return (target: object, key: string) => {
		// % protected region % [Customise validate method here] off begin
		initValidators(target, key);
		target[Symbols.validatorMap][key].push('Url');
		target[Symbols.validator].push(
			(model: Model): Promise<IModelAttributeValidationError | null> => new Promise((resolve) => {
				if (model[key] === null || model[key] === undefined) {
					resolve(null);
				} else {
					resolve(isUrl(model[key])
						? null
						:
						{
							errorType: ErrorType.INVALID,
							errorMessage: `The value is not a valid url`,
							attributeName: key,
							target: model
						});
				}
			})
		);
		// % protected region % [Customise validate method here] end
	};
}


export function isUrl(url: string): boolean {
	return /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/.test(url);
};

// % protected region % [Add any extra methods here] off begin
// % protected region % [Add any extra methods here] end

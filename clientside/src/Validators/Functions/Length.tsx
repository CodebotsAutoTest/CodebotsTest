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
import { Model } from 'Models/Model';
import { Symbols } from 'Symbols';
import { ErrorType, IModelAttributeValidationError, initValidators } from '../Util';

export default function validate(min?: number, max?: number) {
	return (target: object, key: string) => {
		initValidators(target, key);
		target[Symbols.validatorMap][key].push('Length');
		target[Symbols.validator].push(
			(model: Model): Promise<IModelAttributeValidationError | null> => new Promise((resolve) => {
				if (model[key] === null || model[key] === undefined || (!min && !max)) {
					resolve(null);
					return;
				}

				let isValid = true;
				if (min && (model[key].length < min)) {
					isValid = false;
				}
				if (max && model[key].length > max) {
					isValid = false;
				}
				if (isValid) {
					resolve(null);
					return;
				}

				let errorMessage;
				if (min && max) {
					errorMessage = `The length of this field is not ${min} and ${max}. Actual Length: ${model[key].length}`;
				} else if (min) {
					errorMessage = `The length of this field is not greater than ${min}. Actual Length: ${model[key].length}`;
				} else {
					errorMessage = `The length of this field is not less than ${max}. Actual Length: ${model[key].length}`;
				}

				resolve({
					errorType: ErrorType.LENGTH, 
					errorMessage, 
					attributeName: key, 
					target: model 
				});
				return;
			})
		);
	};
}
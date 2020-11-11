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
import { isEmail } from 'Validators/Functions/Email';

const EmailValidatorTheoryData = [
	["a", false],
	["a@examplecom", false],
	["a@.com", false],
	["a@", false],
	["", false],
	["test@@@example.com", false],
	["a@example.com", true],
	["test@example.org", true],
	["test@example.com.au", true],
	["test@example.edu.au", true],
];

describe('Email Validators', () => {
	test.each(EmailValidatorTheoryData)('we expect %p, email validation to be %p', (inputString, expectedValidation) => {
		if (typeof(inputString) === 'string'){
			expect(isEmail(inputString)).toEqual(expectedValidation);
		} else {
			expect(false);
		}
	});
});
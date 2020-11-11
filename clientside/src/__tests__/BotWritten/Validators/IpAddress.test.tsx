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

import { isIP } from 'Validators/Functions/IpAddress';

const IPAddressValidatorTheoryData = [
	['127.0.0.1', true],
	['192.168.1.1', true],
	['192.168.1.255', true],
	['255.255.255.255', true],
	['0.0.0.0', true],
	['1.1.1.01', true],
	['30.168.1.255.1', false],
	['127.1', false],
	['192.168.1.256', false],
	['-1.2.3.4', false],
	['1.1.1.1.', false],
	['3...3', false],
	['', false],
	['sdfsdfsdfsdf', false],
	['-brr', false],
];

describe('IPAddress Validators', () => {
	test.each(IPAddressValidatorTheoryData)('we expect %p, ip validation to be %p', (inputString, expectedValidation) => {
		if (typeof (inputString) === 'string') {
			expect(isIP(inputString)).toEqual(expectedValidation);
		} else {
			expect(false);
		}
	});
});

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
import React from "react";
import { Checkbox } from 'Views/Components/Checkbox/Checkbox'
import { mount } from 'enzyme';

describe('Checkbox Component', () => {
	it('should append -field to the id of the checkbox input', () => {
		let model = {
			"ticked": true
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
			onAfterChecked: () => model.ticked = !model.ticked
		}
		const component = mount(<Checkbox {...props}/>);
	
		expect(component.find('#testCheckbox-field')).toHaveLength(1);
	});
});

describe('Checkbox Component', () => {
	it('a change event on the checkbox triggers on checked function', () => {
		let model = {
			"ticked": true,
			"anotherProperty": "no"
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
			onAfterChecked: () => model.anotherProperty = "yes"
		}
		const component = mount(<Checkbox {...props}/>);
	
		component.find('#testCheckbox-field').simulate('change');
		expect(model.anotherProperty).toEqual("yes");
	});
});

describe('Checkbox Component', () => {
	it('the model property is ommitted, ticked should be false', () => {
		let model = {
			"ticked": true
		}

		const props = {
			model:model,
			modelProperty:"",
			id: "testCheckbox",
		}
		const component = mount(<Checkbox {...props}/>);
		expect(component.find('#testCheckbox-field').prop('checked')).toEqual(false);
	});
});


const StandardBooleanTheoryData = [
	[true],
	[false],
];

describe('Checkbox Component', () => {
	test.each(StandardBooleanTheoryData)('when the model property is %p, we expect checked property to be %p', (expectedOutput) => {
		let model = {
			"ticked": expectedOutput
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
		}
		const component = mount(<Checkbox {...props}/>);
		expect(component.find('#testCheckbox-field').prop('checked')).toEqual(expectedOutput);
	});
});

describe('Checkbox Component', () => {
	test.each(StandardBooleanTheoryData)('when the checkbox is %p from a change, we expect ticked to be %p', (expectedOutput) => {
		let model = {
			"ticked": true
		}

		const props = {
			model:model,
			modelProperty:"ticked",
			id: "testCheckbox",
			isDisabled: false
		}
		const component = mount(<Checkbox {...props}/>);

		component.find('#testCheckbox-field').simulate('change', {target: {checked: expectedOutput}});
		expect(model.ticked).toEqual(expectedOutput);
	});
});
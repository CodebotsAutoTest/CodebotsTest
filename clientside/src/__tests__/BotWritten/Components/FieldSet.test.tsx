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
import ReactDOM from 'react-dom';
import { act } from 'react-dom/test-utils';
import { FieldSet } from 'Views/Components/FieldSet/FieldSet';

let container: HTMLElement | null;

beforeEach(() => {
	container = document.createElement('div');
	document.body.appendChild(container);
});

afterEach(() => {
	if (container) {
		document.body.removeChild(container);
		container = null;
	}
});

describe("FieldSet component", () => {
	it("Display Legend tag and content of the fieldSet", () => {
		act(() => {
			ReactDOM.render(<FieldSet
				id='1'
				name='ExampleName'
				className='example-name'
			>
				<span>Example Content</span>
			</FieldSet>, container);
		});

		if (!!container) {
			const legend = container.querySelector('fieldSet.example-name legend');
			expect(legend).not.toBeNull();
			if(!!legend){
				expect(legend.textContent).toBe('ExampleName');
			}
			const content = container.querySelector('span');
			expect(content).not.toBeNull();
			if(!!content){
				expect(content.textContent).toBe('Example Content');
			}
		}
	});

});

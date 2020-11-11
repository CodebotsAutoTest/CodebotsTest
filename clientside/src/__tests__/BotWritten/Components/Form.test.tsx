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
import { Form } from 'Views/Components/Form/Form';
import { Button } from 'Views/Components/Button/Button';
import { TextField } from 'Views/Components/TextBox/TextBox';


let container: HTMLElement | null;

beforeEach(() => {
	container = document.createElement('div');
	document.body.appendChild(container);
});

afterEach(() => {
	if(container){
		document.body.removeChild(container);
		container = null;
	}
});

describe("Form component", () => {
	it("Display Submit Button", () => {
		act(() => {
			ReactDOM.render(<Form submitButton={true} />, container);
		});

		if(!!container){
			const submitButton = container.querySelector('.submit');
			expect(submitButton).toBeInstanceOf(HTMLButtonElement);
		}
	});

	it("Hide Submit Button", () => {
		act(() => {
			ReactDOM.render(<Form />, container);
		});

		if(!!container){
			const submitButton = container.querySelector('.submit');
			expect(submitButton).toBeNull();
		}
	});

	it("Display Cancel Button", () => {
		act(() => {
			ReactDOM.render(<Form cancelButton={true} />, container);
		});

		if(!!container){
			const cancelButton = container.querySelector('.cancel');
			expect(cancelButton).toBeInstanceOf(HTMLButtonElement);
		}
	});

	it("Hide Cancel Button", () => {
		act(() => {
			ReactDOM.render(<Form />, container);
		});

		if(!!container){
			const cancelButton = container.querySelector('.cancel');
			expect(cancelButton).toBeNull();
		}
	});

	it("Display passed in Action Groups instead of the default actions", () => {
		act(() => {
			const actionGroups = [<Button className='custom-action'>Custom Action</Button>];
			ReactDOM.render(<Form submitButton={true} cancelButton={true} actionGroups={actionGroups} />, container);
		});

		if(!!container){
			const customButton = container.querySelector('.custom-action');
			expect(customButton).toBeInstanceOf(HTMLButtonElement);

			const submitButton = container.querySelector('.submit');
			expect(submitButton).toBeNull();

			const cnacelButton = container.querySelector('.cancel');
			expect(cnacelButton).toBeNull();
		}
	});

	it("Trigger onSubmit callback function", () => {
		const onSubmitMockFunc = jest.fn();

		act(() => {
			ReactDOM.render(<Form 
				submitButton={true} 
				cancelButton={true} 
				onSubmit={onSubmitMockFunc}
			/>, container);
		});

		if(!!container){
			const submitButton = container.querySelector('.action-bar .submit');
			if(!!submitButton){
				submitButton.dispatchEvent(new MouseEvent('click', { bubbles: true }));
				expect(onSubmitMockFunc).toHaveBeenCalled();
			}
		}
	});

	it("Trigger onCancel callback function", () => {
		const onCancelMockFunc = jest.fn();

		act(() => {
			ReactDOM.render(<Form 
				submitButton={true} 
				cancelButton={true} 
				onCancel={onCancelMockFunc}
			/>, container);
		});

		if(!!container){
			const cancelButton = container.querySelector('.action-bar .cancel');
			if(!!cancelButton){
				cancelButton.dispatchEvent(new MouseEvent('click', { bubbles: true }));
				expect(onCancelMockFunc).toHaveBeenCalled();
			}
		}
	});

	it("Display content inside form", () => {
		act(() => {
			const formContent = [<TextField model={{field1: 1}} modelProperty='field1' className='custom-input' key={1}/>];
			ReactDOM.render(<Form submitButton={true} cancelButton={true}>{formContent}</Form>, container);
		});

		if(!!container){
			const customButton = container.querySelector('.crud__form-container .custom-input');
			expect(customButton).toBeInstanceOf(HTMLDivElement);
		}
	});
});

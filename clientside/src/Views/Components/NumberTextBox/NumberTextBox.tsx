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
import * as React from "react";
import { observer } from 'mobx-react';
import { action, computed, observable } from 'mobx';
import { TextField, ITextFieldProps } from '../TextBox/TextBox';

const errorMessage = 'The value must be a number';

type numberModel = {
	number?: string,
};

/**
 * A text field that will cast the value typed into it to a number and will not allow non numbers to be typed into it
 */
@observer
export class NumberTextField<T> extends React.Component<ITextFieldProps<T>> {
	/**
	 * An internal model that contains the string value of the number that is stored in the real model
	 * This allows for values such as '1.' to be displayed even though it is really just '1'
	 */
	@observable
	private model: numberModel = {number: undefined};

	/**
	 * The string value of the text field so the display value does not get mutated unexpectedly
	 */
	@observable
	private stringValue: string = '';

	/**
	 * An error message to display in the case of an invalid number
	 */
	@observable
	private error?: string = undefined;

	/**
	 * A combination of any provided errors and the number error
	 */
	@computed
	private get allErrors() {
		if (this.props.errors) {
			if (Array.isArray(this.props.errors)) {
				return this.error
					? [this.error, ...this.props.errors]
					: this.props.errors;
			}
			return this.error
				? [this.error, this.props.errors]
				: this.props.errors;
		}
		return this.error;
	}

	public render() {
		const modelValue = this.props.model[this.props.modelProperty];
		const stringValue = this.model.number;

		let modelProps;
		if (
			modelValue === Number(stringValue) ||
			(modelValue === undefined && stringValue === '-')
		) {
			modelProps = {
				model: this.model,
				modelProperty: 'number',
			}
		} else {
			modelProps = {
				model: this.props.model,
				modelProperty: this.props.modelProperty,
			}
		}

		return <TextField
			{...this.props}
			{...modelProps}
			onClickToClear={this.onClickToClear}
			errors={this.allErrors}
			inputProps={{
				onChange: this.onChange,
				...this.props.inputProps,
			}} />;
	}

	@action
	private onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		const textValue = event.target.value.trim();
		const numberValue = Number(textValue);

		// Check if the input is actually a number
		if (isNaN(numberValue) && textValue !== '-') {
			this.error = errorMessage;
			return;
		}

		// Clear errors since we know we have a real number
		this.error = undefined;

		// Assign the correct text value
		if (textValue === '' || textValue === '-') {
			this.props.model[this.props.modelProperty] = undefined;
		} else {
			this.props.model[this.props.modelProperty] = numberValue;
		}
		this.model.number = textValue;

		if (this.props.onAfterChange) {
			this.props.onAfterChange(event);
		}
	}

	@action
	private onClickToClear = (event: React.MouseEvent<HTMLButtonElement>) => {
		if (this.props.onClickToClear) {
			return this.props.onClickToClear(event);
		}
		this.props.model[this.props.modelProperty] = '';
	}
}